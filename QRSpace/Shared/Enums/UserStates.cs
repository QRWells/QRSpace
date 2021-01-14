using System;

namespace QRSpace.Shared.Enums
{
    [Flags]
    public enum UserStates : uint
    {
        Online = 0x1,

        Away = 0x1 << 1,

        WaitingForShogi = 0x1 << 2,
    }
}