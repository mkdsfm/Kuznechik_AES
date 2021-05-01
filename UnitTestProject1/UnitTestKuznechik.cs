using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using lab2._1;

namespace UnitTestKuznechik
{
    [TestClass]
    public class UnitTestKuznechik
    {
        [TestMethod]
        public void TestMethod1()
        {
            KuznechikCipher kuz = new KuznechikCipher();
            string key = "234";
            string textO = "ккк";
            string textEncodeFact = kuz.Encode(textO, key);
            string textEncode = "160 150 169 132 128 105 106 20 206 43 47 31 165 147 90 231";
            string textDecodeFact = kuz.Decode(textEncode, key);
            textDecodeFact = textDecodeFact.Substring(0, textO.Length);
            Assert.AreEqual(textO, textDecodeFact);
            Assert.AreEqual(textEncode, textEncodeFact);
        }
        [TestMethod]
        public void TestMethod2()
        {
            KuznechikCipher kuz = new KuznechikCipher();
            string key = "234";
            string textO = "привет";
            string textEncodeFact = kuz.Encode(textO, key);
            string textEncode = "16 73 116 170 47 111 92 152 224 242 240 158 164 154 60 51";
            string textDecodeFact = kuz.Decode(textEncode, key);
            textDecodeFact = textDecodeFact.Substring(0, textO.Length);
            Assert.AreEqual(textO, textDecodeFact);
            Assert.AreEqual(textEncode, textEncodeFact);
        }
    }
}
