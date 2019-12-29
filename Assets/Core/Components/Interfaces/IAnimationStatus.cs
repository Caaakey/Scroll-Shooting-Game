using System;
using System.Collections;
using System.Collections.Generic;

namespace SSAction.Core.Interfaces
{
    [Flags]
    public enum AnimStatus : int
    {
        Idle = 0,
        Run,
        Jump,
        Sitdown,
        MeleeAttack,
        Rolling
    }

    class AnimStatusCompare : IEqualityComparer<AnimStatus>
    {
        public bool Equals(AnimStatus x, AnimStatus y)
            => x == y;

        public int GetHashCode(AnimStatus obj)
            => (int)obj;
    }

    interface IAnimationStatus
    {
        AnimStatus Status { get; set; }
        void ChangeStatus(AnimStatus status, bool isSet);
    }
}

