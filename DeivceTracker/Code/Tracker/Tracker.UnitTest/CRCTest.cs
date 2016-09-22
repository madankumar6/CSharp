using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace Tracker.UnitTest
{
    [TestClass]
    public class CRCTest
    {
        [TestMethod]
        public void SampleDataByDevice()
        {
            byte[] loginData = {
                0x78, 0x78,
                0x0D,
                0x01,
                0x03, 0x58, 0x89, 0x90, 0x55, 0x87, 0x57, 0x39,

                0x00, 0x02,

                0xAC, 0x1B,

                0x0D, 0x0A
            };


            byte[] forCheckSumData = {
                0x0D,
                0x01,
                0x03, 0x58, 0x89, 0x90, 0x55, 0x87, 0x57, 0x39,

                0x00, 0x02,
            };

            var checkSum = CRC16.ComputeChecksumBytes(forCheckSumData);

            var sttr = BitConverter.ToString(checkSum);
            Assert.IsTrue(sttr == "AC-1B");
        }

        [TestMethod]
        public void ReplyLoginData()
        {
            byte[] forCheckSumData = {
                0x05,
                0x01,
                0x00, 0x01
            };

            var checkSum = CRC16.ComputeChecksumBytes(forCheckSumData);


            var sttr = BitConverter.ToString(checkSum);
            Assert.IsTrue(sttr == "D9-DC");
        }

        [TestMethod]
        public void ReplyLoginData_Assume()
        {
            byte[] forCheckSumData = {
                0x05,
                0x01,
                0x00, 0x02
            };

            var checkSum = CRC16.ComputeChecksumBytes(forCheckSumData);


            var sttr = BitConverter.ToString(checkSum);
            Assert.IsTrue(sttr == "EB-47");
        }

        [TestMethod]
        public void ReplyAlarmData()
        {
            byte[] forCheckSumData = {
                0x0A,
                0x26,
                0x00, 0x01
            };

            var checkSum = CRC16.ComputeChecksumBytes(forCheckSumData);


            var sttr = BitConverter.ToString(checkSum);
            Assert.IsTrue(sttr == "E4-1B");
        }


        [TestMethod]
        public void ReplyHeartBeatData()
        {
            byte[] forCheckSumData = {
                0x05,
                0x13,
                0x00, 0x01
            };

            var checkSum = CRC16.ComputeChecksumBytes(forCheckSumData);


            var sttr = BitConverter.ToString(checkSum);
            Assert.IsTrue(sttr == "E9-F1");
        }
    }
}
