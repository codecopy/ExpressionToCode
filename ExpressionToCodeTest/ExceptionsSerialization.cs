﻿#if binary_serialization
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ExpressionToCodeLib;
using Xunit;

namespace ExpressionToCodeTest
{
    public class ExceptionsSerialization
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void IntentionallyFailingMethod()
            => PAssert.That(() => false);

        [Fact]
        public void PAssertExceptionIsSerializable()
            => AssertMethodFailsWithSerializableException(IntentionallyFailingMethod);

        static void AssertMethodFailsWithSerializableException(Action intentionallyFailingMethod)
        {
            var original = Assert.ThrowsAny<Exception>(intentionallyFailingMethod);

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var ms = new MemoryStream();
            formatter.Serialize(ms, original);
            var deserialized = formatter.Deserialize(new MemoryStream(ms.ToArray()));
            Assert.Equal(original.ToString(), deserialized.ToString());
        }
    }
}
#endif
