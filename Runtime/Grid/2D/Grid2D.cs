/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：网格的基类 2D
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using UnityEngine;

namespace HoopyGame
{
    public class Grid2D<TGrid>
    {
        private event EventHandler<OnGridValueChangeEventArgs> OnGridValueChange;
        public class OnGridValueChangeEventArgs : EventArgs
        {
            public int x, y;
        }

        private readonly int _width;
        public int GetWidth => _width;
        private readonly int _height;
        public int GetHeight => _height;
        private Vector2 _cellSize_V2;
        public Vector2 GetCellSize => _cellSize_V2;
        private Vector2 _originPos_V2;
        private TGrid[,] _gridArray;

        public Grid2D(int width, int height, Vector2 cellSize, Vector2 originPos, Func<Grid2D<TGrid>, int, int, TGrid> initTGrid, bool showDebug = false)
        {
            _width = width;
            _height = height;
            _cellSize_V2 = cellSize;
            _originPos_V2 = originPos;
            _gridArray = new TGrid[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _gridArray[x, y] = initTGrid(this, x, y);
                }
            }

            if (showDebug)
            {
                //在地图上绘制出来网格
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);
            }
        }

        /// <summary>
        /// 获取一个点在世界坐标中的位置
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>位置</returns>
        public Vector2 GetWorldPosition(int x, int y)
            => new Vector2(x, y) * _cellSize_V2 + _originPos_V2;
        /// <summary>
        /// 获取一个位置所在的
        /// </summary>
        /// <param name="position"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetIndex(Vector2 position, out int x, out int y)
        {
            x = Mathf.FloorToInt((position.x - _originPos_V2.x) / _cellSize_V2.x);
            y = Mathf.FloorToInt((position.y - _originPos_V2.y) / _cellSize_V2.y);
        }
        /// <summary>
        /// 设置一个格子的值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tGrid"></param>
        public void SetGridValue(int x, int y, TGrid tGrid)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _gridArray[x, y] = tGrid;
                OnGridValueChange?.Invoke(this, new OnGridValueChangeEventArgs { x = x, y = y });
            }
        }
        /// <summary>
        /// 设置一个格子的值
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tGrid"></param>
        public void SetGridValue(Vector2 position, TGrid tGrid)
        {
            GetIndex(position, out int x, out int y);
            SetGridValue(x, y, tGrid);
        }
        /// <summary>
        /// 获取一个格子的值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public TGrid GetGridValue(int x, int y)
            => (x >= 0 && x < _width && y >= 0 && y < _height) ? _gridArray[x, y] : default;
        /// <summary>
        /// 获取一个格子的值
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public TGrid GetGridValue(Vector2 position)
        {
            int x, y;
            GetIndex(position, out x, out y);
            return GetGridValue(x, y);
        }
        /// <summary>
        /// 手动触发某个格子的值的变更事件
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public void TriggerGridValueChangeEvent(int x, int y)
        {
            OnGridValueChange?.Invoke(this, new OnGridValueChangeEventArgs { x = x, y = y });
        }

    }
}