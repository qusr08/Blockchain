using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour {
	[Header(" --- World Manager Class --- ")]
	[SerializeField] private Tilemap floorTiles;
	[SerializeField] private Tilemap edgeTiles;
	[Space]
	[SerializeField] private Sprite[ ] newEdgeTileSprites = new Sprite[46];
	[SerializeField] private Sprite[ ] brokenEdgeTileSprites = new Sprite[46];

	private Dictionary<int, int> edgeTileConversions = new Dictionary<int, int> {
		[1] = 0,
		[2] = 1,
		[4] = 2,
		[5] = 3,
		[8] = 4,
		[10] = 5,
		[12] = 6,
		[16] = 7,
		[17] = 8,
		[18] = 9,
		[24] = 10,
		[26] = 11,
		[32] = 12,
		[33] = 13,
		[34] = 14,
		[36] = 15,
		[37] = 16,
		[48] = 17,
		[49] = 18,
		[50] = 19,
		[64] = 20,
		[65] = 21,
		[66] = 22,
		[68] = 23,
		[69] = 24,
		[72] = 25,
		[74] = 26,
		[76] = 27,
		[80] = 28,
		[81] = 29,
		[82] = 30,
		[88] = 31,
		[90] = 32,
		[128] = 33,
		[129] = 34,
		[130] = 35,
		[132] = 36,
		[133] = 37,
		[136] = 38,
		[138] = 39,
		[140] = 40,
		[161] = 41,
		[160] = 42,
		[162] = 43,
		[164] = 44,
		[165] = 45
	};

	private void OnValidate ( ) {
		if (floorTiles == null) {
			floorTiles = transform.Find("Floor").GetComponent<Tilemap>( );
		}

		if (edgeTiles == null) {
			edgeTiles = transform.Find("Edges").GetComponent<Tilemap>( );
		}
	}

	private void Awake ( ) {
		// Loop through all edge tile positions and calculate what sprite needs to be used for that tile
		foreach (Vector3Int position in floorTiles.cellBounds.allPositionsWithin) {
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {
					// Make sure there isn't already a tile on either of the tilemaps
					// This is for efficiency so a tile that was already set isn't tried to be set again
					Vector3Int loopPosition = new Vector3Int(position.x + x, position.y + y, position.z);
					if (floorTiles.HasTile(loopPosition) || edgeTiles.HasTile(loopPosition)) {
						continue;
					}

					// Calculate bitmask value
					int northWest = 0;
					int north = 2 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.North)));
					int northEast = 0;
					int west = 8 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.West)));
					int east = 16 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.East)));
					int southWest = 0;
					int south = 64 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.South)));
					int southEast = 0;

					if (north == 0 && west == 0) {
						northWest = 1 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.NorthWest)));
					}

					if (north == 0 && east == 0) {
						northEast = 4 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.NorthEast)));
					}

					if (south == 0 && west == 0) {
						southWest = 32 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.SouthWest)));
					}

					if (south == 0 && east == 0) {
						southEast = 128 * Utils.BoolToInt(floorTiles.HasTile(loopPosition + Utils.Vect3ToVect3Int(Utils.SouthEast)));
					}

					int bitmaskValue = northWest + north + northEast + west + east + southWest + south + southEast;

					// If there is no sprite given that matches the bitmask calculation, then just set the tile to null and move on
					if (!edgeTileConversions.ContainsKey(bitmaskValue)) {
						edgeTiles.SetTile(loopPosition, null);

						if (bitmaskValue != 0) {
							Debug.LogError($"Unknown bitmask value: {bitmaskValue}");
						}

						continue;
					}

					// Convert bitmask value to tile value
					int tileIndex = -1;
					edgeTileConversions.TryGetValue(bitmaskValue, out tileIndex);

					// Randomly choose between the edge tile being broken or new
					Sprite edgeSprite = (Utils.GetRandBool( ) ? newEdgeTileSprites : brokenEdgeTileSprites)[tileIndex];

					// Set the edge tile on the tilemap
					Tile tile = (Tile) ScriptableObject.CreateInstance("Tile");
					tile.sprite = edgeSprite;

					edgeTiles.SetTile(loopPosition, tile);
				}
			}
		}

		// Refresh all of the tiles
		edgeTiles.RefreshAllTiles( );
	}
}
