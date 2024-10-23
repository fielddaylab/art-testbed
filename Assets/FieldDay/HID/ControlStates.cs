using System;
using System.Runtime.CompilerServices;
using BeauUtil;

namespace FieldDay.HID {
    /// <summary>
    /// State of a set of digital controls (buttons).
    /// </summary>
    public struct DigitalControlStates {
        public uint Current;
        public uint Prev;

        public uint Pressed;
        public uint Released;

        #region Modifiers

        public bool Update(uint current) {
            Prev = Current;
            Current = current;

            uint changes = Current ^ Prev;
            Pressed = changes & Current;
            Released = changes & (~Current);
            return changes != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() {
            Current = Prev = Pressed = Released = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearChanges() {
            Pressed = Released = 0;
        }

        /// <summary>
        /// Consumes a press.
        /// Returns if it was pressed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConsumePress(uint mask) {
            bool had = (Pressed & mask) != 0;
            Pressed &= ~mask;
            return had;
        }

        /// <summary>
        /// Consumes a release.
        /// Returns if it was released.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConsumeRelease(uint mask) {
            bool had = (Released & mask) != 0;
            Released &= ~mask;
            return had;
        }

        #endregion // Modifiers

        #region Accessors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsDown(uint mask) {
            return (Current & mask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsDownAll(uint mask) {
            return (Current & mask) == mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly  bool IsPressed(uint mask) {
            return (Pressed & mask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsReleased(uint mask) {
            return (Released & mask) != 0;
        }

        #endregion // Accessors

        #region Operators

        static public DigitalControlStates operator |(DigitalControlStates a, DigitalControlStates b) {
            DigitalControlStates s;
            s.Current = a.Current | b.Current;
            s.Prev = a.Prev | b.Prev;
            s.Pressed = a.Pressed | b.Pressed;
            s.Released = a.Released | b.Released;
            return s;
        }

        static public DigitalControlStates operator &(DigitalControlStates a, DigitalControlStates b) {
            DigitalControlStates s;
            s.Current = a.Current & b.Current;
            s.Prev = a.Prev & b.Prev;
            s.Pressed = a.Pressed & b.Pressed;
            s.Released = a.Released & b.Released;
            return s;
        }

        static public DigitalControlStates operator ^(DigitalControlStates a, DigitalControlStates b) {
            DigitalControlStates s;
            s.Current = a.Current ^ b.Current;
            s.Prev = a.Prev ^ b.Prev;
            s.Pressed = a.Pressed ^ b.Pressed;
            s.Released = a.Released ^ b.Released;
            return s;
        }

        #endregion // Operators
    }

    /// <summary>
    /// State of a set of digital controls (buttons), represented by an Enum.
    /// </summary>
    public struct DigitalControlStates<TEnum> where TEnum : unmanaged, Enum {
        public TEnum Current;
        public TEnum Prev;

        public TEnum Pressed;
        public TEnum Released;

        #region Modifiers

        public bool Update(TEnum current) {
            Prev = Current;
            Current = current;

            TEnum changes = Enums.Xor(Current, Prev);;
            Pressed = Enums.And(changes, Current);
            Released = Enums.And(changes, Enums.Not(Current));
            return Enums.IsNotZero(changes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() {
            Current = Prev = Pressed = Released = default(TEnum);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearChanges() {
            Pressed = Released = default(TEnum);
        }

        /// <summary>
        /// Consumes a press.
        /// Returns if it was pressed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConsumePress(TEnum mask) {
            bool had = Enums.IsNotZero(Enums.And(Pressed, mask));
            Pressed = Enums.And(Pressed, Enums.Not(mask));
            return had;
        }

        /// <summary>
        /// Consumes a release.
        /// Returns if it was released.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConsumeRelease(TEnum mask) {
            bool had = Enums.IsNotZero(Enums.And(Released, mask));
            Released = Enums.And(Released, Enums.Not(mask));
            return had;
        }

        #endregion // Modifiers

        #region Accessors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsDown(TEnum mask) {
            return Enums.IsNotZero(Enums.And(Current, mask));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsDownAll(TEnum mask) {
            return Enums.AreEqual(Enums.And(Current, mask), mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsPressed(TEnum mask) {
            return Enums.IsNotZero(Enums.And(Pressed, mask));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsReleased(TEnum mask) {
            return Enums.IsNotZero(Enums.And(Released, mask));
        }

        #endregion // Accessors

        #region Operators

        static public DigitalControlStates<TEnum> operator |(DigitalControlStates<TEnum> a, DigitalControlStates<TEnum> b) {
            DigitalControlStates<TEnum> s;
            s.Current = Enums.Or(a.Current, b.Current);
            s.Prev = Enums.Or(a.Prev, b.Prev);
            s.Pressed = Enums.Or(a.Pressed, b.Pressed);
            s.Released = Enums.Or(a.Released, b.Released);
            return s;
        }

        static public DigitalControlStates<TEnum> operator &(DigitalControlStates<TEnum> a, DigitalControlStates<TEnum> b) {
            DigitalControlStates<TEnum> s;
            s.Current = Enums.And(a.Current, b.Current);
            s.Prev = Enums.And(a.Prev, b.Prev);
            s.Pressed = Enums.And(a.Pressed, b.Pressed);
            s.Released = Enums.And(a.Released, b.Released);
            return s;
        }

        static public DigitalControlStates<TEnum> operator ^(DigitalControlStates<TEnum> a, DigitalControlStates<TEnum> b) {
            DigitalControlStates<TEnum> s;
            s.Current = Enums.Xor(a.Current, b.Current);
            s.Prev = Enums.Xor(a.Prev, b.Prev);
            s.Pressed = Enums.Xor(a.Pressed, b.Pressed);
            s.Released = Enums.Xor(a.Released, b.Released);
            return s;
        }

        #endregion // Operators
    }

    /// <summary>
    /// State of an axis.
    /// </summary>
    public struct AxisControlState8 {
        public float Raw;
        public float Adjusted;
        public unsafe fixed float AdjustedHistory[8];
    }
}