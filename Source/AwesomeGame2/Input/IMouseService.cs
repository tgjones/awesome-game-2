using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2.Input
{
    public interface IMouseService
    {
        int ScrollWheelValueChange { get; }
				bool LeftClickPressed { get; }
        bool RightClickPressed { get; }
				int X { get; }
				int Y { get; }
    }
}
