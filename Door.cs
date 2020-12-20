using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Rage.Native;

namespace CayoPericoRPH.Doors
{
    /// <summary>
    /// Simple wrapper class for door natives
    /// pastebin with all door hashes found in the scripts: https://pastebin.com/9S2m3qA4 and https://pastebin.com/gywnbzsH
    /// </summary>
    public class Door
    {
        private uint _doorHash;
        public uint DoorHash
        {
            get => _doorHash;
            set => _doorHash = value;
        }

        private Vector3 _position;

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public bool Exists => DoorHash != 0;

        private float _heading;
        private bool _locked;

        public Door(uint doorHash, Vector3 position)
        {
            _doorHash = doorHash;
            _position = position;
        }

        public Door(Rage.Object doorEntity)
        {
            InitializeFromEntity(doorEntity);
        }

        public Door(Vector3 position)
        {
            Rage.Object doorEntity = World.GetAllObjects().Where(o => o.Model.Name.ToLower().Contains("door")).OrderBy(o => o.DistanceTo(position)).FirstOrDefault();
            if (doorEntity)
            {
                InitializeFromEntity(doorEntity);
            }
            else
            {
                Game.LogTrivial($"Couldn't initialize the door for position {position}");
                DoorHash = 0;
                return;
            }
        }

        public void InitializeFromEntity(Rage.Object doorEntity)
        {
            _doorHash = doorEntity.Model;
            _position = doorEntity.Position;
        }

        public bool IsPhysicsLoaded => NativeFunction.CallByHash<bool>(0xDF97CDD4FC08FD34, _doorHash); //_DOOR_SYSTEM_GET_IS_PHYSICS_LOADED

        public float OpenRatio
        {
            get => NativeFunction.Natives.x65499865FCA6E5EC<float>(_doorHash); //_DOOR_SYSTEM_GET_OPEN_RATIO
            set => NativeFunction.Natives.xB6E6FBA95C7324AC(_doorHash, value, true, true);
        }

        public float AutomaticDistance
        {
            set => NativeFunction.Natives.x9BA001CB45CBF627(_doorHash, value, true, true); //_DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE
        }

        public float AutomaticRate
        {
            set => NativeFunction.Natives.x03C27E13B42A0E82(_doorHash, value, true, true);
        }

        public bool IsClosed => NativeFunction.CallByHash<bool>((ulong)Hash.IS_DOOR_CLOSED, _doorHash);

        public bool IsLocked
        {
            get
            {
                bool locked;
                float heading;

                NativeFunction.Natives.xEDC1A5B84AEF33FF(_doorHash, _position.X, _position.Y, _position.Z, out locked, out heading); //GET_STATE_OF_CLOSEST_DOOR_OF_TYPE

                return locked;
            }
            set
            {
                NativeFunction.CallByHash((ulong)Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, typeof(int), _doorHash, _position.X, _position.Y,
                    _position.Z, value, _heading, false);

                _locked = value;
            }
        }

        public float Heading
        {
            get
            {
                bool locked;
                float heading;

                NativeFunction.Natives.xEDC1A5B84AEF33FF(_doorHash, _position.X, _position.Y, _position.Z, out locked, out heading); //GET_STATE_OF_CLOSEST_DOOR_OF_TYPE

                return heading;

            }
            set
            {
                NativeFunction.CallByHash((ulong)Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, typeof(int), _doorHash, _position.X, _position.Y, _position.Z,
                    _locked, value, false);

                _heading = value;
            }
        }

        public EDoorState PendingState
        {
            get
            {
                var state = NativeFunction.CallByHash<int>(0x4BC2854478F3A749, _doorHash); //_DOOR_SYSTEM_GET_DOOR_PENDING_STATE

                return (EDoorState)state;
            }
        }

        public EDoorState State
        {
            get
            {
                var state = NativeFunction.CallByHash<int>(0x160AA1B32F6139B8, _doorHash); //_DOOR_SYSTEM_GET_DOOR_STATE

                return (EDoorState)state;
            }
            set => NativeFunction.Natives.x6BAB9442830C7F53(_doorHash, (int)value, true, true);
        }

        public bool IsInMemory => NativeFunction.CallByHash<bool>((ulong)Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, _doorHash);

        public enum EDoorState
        {
            Unlocked = 0,
            Locked = 1,
            ForceLockedUntilOutOfArea = 2,
            ForceUnlockedThisFrame = 3,
            ForceLockedThisFrame = 4,
            ForceOpenThisFrame = 5,
            ForceClosedThisFrame = 6
        }
    }
}
