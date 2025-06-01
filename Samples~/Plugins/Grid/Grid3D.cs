/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：网格的基类 3D
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
    public class Grid3D<TGrid>
    {
        private event EventHandler<OnGridValueChangeEventArgs> OnGridValueChange;
        public class OnGridValueChangeEventArgs : EventArgs
        {
            public int x, y, z;
        }
        private readonly int _width;
        public int GetWidth => _width;

        private readonly int _height;
        public int GetHeight => _height;

        private readonly int _depth;
        public int GetDepth => _depth;

        private Vector3 _cellSize_V3;
        public Vector2 GetCellSize => _cellSize_V3;

        private Vector3 _originPos_V3;
        private TGrid[,,] _gridArray;

        public Grid3D(
            int width,
            int height,
            int depth,
            Vector3 cellSize,
            Vector3 originPos,
            Func<Grid3D<TGrid>, int, int, int, TGrid> initGrid,
            bool showDebug = false)
        {
            _width = width;
            _height = height;
            _depth = depth;
            _cellSize_V3 = cellSize;
            _originPos_V3 = originPos;
            _gridArray = new TGrid[width, height, depth];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        _gridArray[x, y, z] = initGrid(this, x, y, z);
                    }
                }
            }

            if (showDebug)
            {
                //在地图上绘制出来网格
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.white, 100f);
                            Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.white, 100f);
                            Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.white, 100f);

                            Debug.DrawLine(GetWorldPosition(x, height, 0), GetWorldPosition(x, height, z + 1), Color.white, 100f);
                            Debug.DrawLine(GetWorldPosition(x, 0, depth), GetWorldPosition(x, y + 1, depth), Color.white, 100f);

                            Debug.DrawLine(GetWorldPosition(0, y, depth), GetWorldPosition(width, y, depth), Color.white, 100f);
                            Debug.DrawLine(GetWorldPosition(width, y, 0), GetWorldPosition(width, y, depth), Color.white, 100f);

                            Debug.DrawLine(GetWorldPosition(width, 0, z), GetWorldPosition(width, height, z), Color.white, 100f);
                            Debug.DrawLine(GetWorldPosition(0, height, z), GetWorldPosition(width, height, z), Color.white, 100f);

                        }
                    }
                }
                Debug.DrawLine(GetWorldPosition(width, height, 0), GetWorldPosition(width, height, depth), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(0, height, depth), GetWorldPosition(width, height, depth), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0, depth), GetWorldPosition(width, height, depth), Color.white, 100f);
            }
        }

        /// <summary>
        /// 获取一个点在世界坐标中的位置
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>位置</returns>
        public Vector3 GetWorldPosition(int x, int y, int z)
            => new Vector3(x * _cellSize_V3.x, y * _cellSize_V3.y, z * _cellSize_V3.z) + _originPos_V3;
        /// <summary>
        /// 获取一个位置所在的
        /// </summary>
        /// <param name="position"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetIndex(Vector3 position, out int x, out int y, out int z)
        {
            x = Mathf.FloorToInt((position.x - _originPos_V3.x) / _cellSize_V3.x);
            y = Mathf.FloorToInt((position.y - _originPos_V3.y) / _cellSize_V3.y);
            z = Mathf.FloorToInt((position.z - _originPos_V3.z) / _cellSize_V3.z);
        }
        /// <summary>
        /// 设置一个格子的值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tGrid"></param>
        public void SetGridValue(int x, int y, int z, TGrid tGrid)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height && z >= 0 && z < _depth)
            {
                _gridArray[x, y, z] = tGrid;
                OnGridValueChange?.Invoke(this, new OnGridValueChangeEventArgs { x = x, y = y, z = z });
            }
        }
        /// <summary>
        /// 设置一个格子的值
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tGrid"></param>
        public void SetGridValue(Vector3 position, TGrid tGrid)
        {
            GetIndex(position, out int x, out int y, out int z);
            SetGridValue(x, y, z, tGrid);
        }
        /// <summary>
        /// 获取一个格子的值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public TGrid GetGridValue(int x, int y, int z)
            => (x >= 0 && x < _width && y >= 0 && y < _height && z >= 0 && z < _depth) ? _gridArray[x, y, z] : default;
        /// <summary>
        /// 获取一个格子的值
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public TGrid GetGridValue(Vector3 position)
        {
            GetIndex(position, out int x, out int y, out int z);
            return GetGridValue(x, y, z);
        }
        /// <summary>
        /// 手动触发某个格子的值的变更事件
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void TriggerGridValueChangeEvent(int x, int y, int z)
        {
            OnGridValueChange?.Invoke(this, new OnGridValueChangeEventArgs { x = x, y = y, z = z });
        }
    }
}