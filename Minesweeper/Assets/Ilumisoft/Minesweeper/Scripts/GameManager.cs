using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilumisoft.Minesweeper
{
    public class GameManager : MonoBehaviour, ITileClickListener
    {
        [SerializeField]
        GameObject levelCompleteUI = null;

        [SerializeField]
        GameObject gameOverUI = null;

        [SerializeField]
        Tile normalTilePrefab = null;

        [SerializeField]
        Tile bombTilePrefab = null;

        [SerializeField]
        TileGrid grid = null;

        [SerializeField]
        int bombCount = 5;

        GameObject tileContainer = null;

        List<Tile> tiles = new List<Tile>();

        private void Awake()
        {
            tileContainer = new GameObject("Tile Container");
        }

        void Start()
        {
            CreateBoard();
        }

        void CreateBoard()
        {
            AddBombsToGrid();
            AddNormalTilesToGrid();

            foreach (var tile in tiles)
            {
                if (tile.TryGetComponent<TileNumber>(out var tileNumber))
                {
                    tileNumber.SetNumberOfBombs(grid.GetNumberOfSurroundingBombs(tile));
                }
            }
        }

        private void AddBombsToGrid()
        {
            bombCount = Mathf.Min(bombCount, grid.Width * grid.Height);

            for (int i = 0; i < bombCount; i++)
            {
                int x = Random.Range(0, grid.Width);
                int y = Random.Range(0, grid.Height);

                if (grid.TryGetTile(x, y, out _) == false)
                {
                    AddTileToGrid(x, y, bombTilePrefab);
                }
                else
                {
                    i--;
                    continue;
                }
            }
        }

        private void AddNormalTilesToGrid()
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid.TryGetTile(x, y, out _) == false)
                    {
                        AddTileToGrid(x, y, normalTilePrefab);
                    }
                }
            }
        }

        void AddTileToGrid(int x, int y, Tile prefab)
        {
            var position = grid.GetWorldPosition(x, y);

            var tile = Instantiate(prefab, position, Quaternion.identity);
            tile.transform.SetParent(tileContainer.transform);
            grid.SetTile(x, y, tile);

            tiles.Add(tile);
        }

        public void OnTileClick(Tile tile)
        {
            if (tile.State == TileState.Hidden)
            {
                TileRevealer tileRevealer = new TileRevealer(grid);

                tileRevealer.Reveal(tile);

                if (tile.CompareTag(Bomb.Tag))
                {
                    GameOver(won: false);
                }
                else if(HasRevealedAllSafeTiles())
                {
                    GameOver(won: true);
                }
            }
        }

        bool HasRevealedAllSafeTiles()
        {
            foreach (var tile in tiles)
            {
                if (tile.CompareTag(Bomb.Tag) == false && tile.State != TileState.Revealed)
                {
                    return false;
                }
            }

            return true;
        }

        void GameOver(bool won)
        {
            StopAllCoroutines();
            FindObjectOfType<AudioManager>().Play("Square Open Three");
            StartCoroutine(GameOverCoroutine(won));
        }

        IEnumerator GameOverCoroutine(bool won)
        {
            GameObject uiElement = won ? levelCompleteUI : gameOverUI;

            yield return new WaitForSecondsRealtime(1.0f);
            
            if(won == false)
            FindObjectOfType<AudioManager>().Play("Explosion");

            uiElement.SetActive(true);
        }
    }
}