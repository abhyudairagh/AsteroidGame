using System;
using Managers;
using Managers.Impl;
using UnityEngine;
using Random = System.Random;


namespace Helpers.Impl
{
    public class SpawningPositionUtility : ISpawningPositionUtility
    {
        private readonly Random _rand = new Random();

        private readonly IScreenUtility _screenUtility;

        public SpawningPositionUtility(IScreenUtility screenUtility)
        {
            _screenUtility = screenUtility;
        }
    
        public Vector2 GetAsteroidRandomSpawningPosition()
        {
            var side = RandomEnumValue<ScreenSide>();
            Vector2 position = new Vector2();
            switch (side)
            {
                case ScreenSide.Right:

                    position.x = _screenUtility.RightTopCorner.x + 2f;
                    position.y = _screenUtility.GetRandomPointScreenHeight();
                    break;

                case ScreenSide.Left:
                    position.x = _screenUtility.LeftBottomCorner.x - 2f;
                    position.y = _screenUtility.GetRandomPointScreenHeight();
                    break;

                case ScreenSide.Top:
                    position.x = _screenUtility.GetRandomPointScreenWidth();
                    position.y = _screenUtility.RightTopCorner.y + 2f;
                    break;

                case ScreenSide.Bottom:
                    position.x = _screenUtility.GetRandomPointScreenWidth();
                    position.y = _screenUtility.LeftBottomCorner.y - 2f;
                    break;

            }

            return position;
        }
    
    
        public T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_rand.Next(v.Length));
        }
    

        /// <summary>
        /// Get random position inside screen
        /// </summary>
        /// <param name="paddingX"></param>
        /// <param name="paddingY"></param>
        /// <returns></returns>
        public Vector2 GetRandomSpawningPointInsideScreen(float paddingX = 0, float paddingY = 0)
        {
            Vector2 position = new Vector2();
            position.x = _screenUtility.GetRandomPointScreenWidth(paddingX);
            position.y = _screenUtility.GetRandomPointScreenHeight(paddingY);
            return position;
        }
    }
}