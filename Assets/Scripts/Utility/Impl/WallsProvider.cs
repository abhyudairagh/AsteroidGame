using System;
using System.Collections.Generic;
using Factory;
using Managers;
using Managers.Impl;
using UnityEngine;
using Zenject;

namespace Utility.Impl
{
    public class WallsProvider : MonoBehaviour, IWallsProvider
    {
        [SerializeField]
        private float colliderThickness;
           
        [SerializeField]
        private float  posoffset;
        
        private Vector2 RightTopCorner => _screenUtility.RightTopCorner;
        private Vector2 LeftBottomCorner => _screenUtility.LeftBottomCorner;
        
        private float _lengthOffset ;
        private float _offsetPosition;

        private readonly Dictionary<ScreenSide, BoxCollider2D> _wallsColliders = new();
        
        private IScreenUtility _screenUtility;
        private IWallFactory _wallFactory;
        
        [Inject]
        public void Construct(IScreenUtility screenUtility, IWallFactory wallFactory)
        {
            _screenUtility = screenUtility;
            _wallFactory = wallFactory;
            
            Initialise();
        }

        private void Initialise()
        {
            _offsetPosition = (colliderThickness * 0.5f) + posoffset;
            _lengthOffset = colliderThickness * 2f + (posoffset * 2f);

            _screenUtility.OnScreenInitialised += OnScreenSizeChanged;
            _screenUtility.OnScreenSizeChanged += OnScreenSizeChanged;

            CreateWalls();
        }

        private void OnScreenSizeChanged()
        {
            UpdateColliders();
        }

        private void CreateWalls()
        {
            foreach (var value in Enum.GetValues(typeof(ScreenSide)))
            {
                var wall = _wallFactory.GenerateBoxWall((ScreenSide)value, transform);
                _wallsColliders.Add((ScreenSide)value, wall);
            }
        }
        
        void UpdateColliders()
        {
            foreach (var (side, wall) in _wallsColliders)
            {
                wall.transform.position = GetColliderPosition(side);
                wall.size = GetColliderSize(side);
            }
        }

        private Vector3 GetColliderPosition(ScreenSide side)
        {
            return side switch
            {
                ScreenSide.Top => new Vector3(0, RightTopCorner.y + _offsetPosition, 0),
                ScreenSide.Bottom => new Vector3(0, LeftBottomCorner.y - _offsetPosition, 0),
                ScreenSide.Left => new Vector3(LeftBottomCorner.x - _offsetPosition, 0, 0),
                ScreenSide.Right => new Vector3(RightTopCorner.x + _offsetPosition, 0, 0),
                _ => default
            };
        }

        private Vector2 GetColliderSize(ScreenSide side)
        {
            return side switch
            {
                ScreenSide.Top => new Vector2((RightTopCorner.x * 2f) + _lengthOffset, colliderThickness),
                ScreenSide.Bottom => new Vector2((RightTopCorner.x * 2f) + _lengthOffset, colliderThickness),
                ScreenSide.Left => new Vector2(colliderThickness, (RightTopCorner.y * 2f) + _lengthOffset),
                ScreenSide.Right => new Vector2(colliderThickness, (RightTopCorner.y * 2f) + _lengthOffset),
                _ => default
            };
        }
        
        /// <summary>
        /// Returns true if a point is inside the collider walls
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsInsideWalls(Vector2 pos)
        {
            var leftBottomWallCorner = GetLeftBottomWallCorners();
            var rightTopWallCorner = GetRightTopWallCorners();
            return (pos.x > leftBottomWallCorner.x && pos.x < rightTopWallCorner.x && pos.y > leftBottomWallCorner.y && pos.y < rightTopWallCorner.y) ;
        }
        
         
        /// <summary>
        /// Gets the left bottom corner world coordinates of the screen 
        /// </summary>
        /// <returns></returns>
        private Vector2 GetLeftBottomWallCorners()
        {
            return new Vector2(LeftBottomCorner.x - _offsetPosition, LeftBottomCorner.y - _offsetPosition);
        }

        /// <summary>
        /// Gets the right top corner world coordinates of the screen 
        /// </summary>
        /// <returns></returns>
        private Vector2 GetRightTopWallCorners()
        {
            return new Vector2(RightTopCorner.x + _offsetPosition, RightTopCorner.y + _offsetPosition);
        }
    }
}