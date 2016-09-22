using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker.TcpClientTest.Data
{
    class Gt00SampleData : ISampleData
    {
        Dictionary<string, object> gt100Data = new Dictionary<string, object>();

        public Gt00SampleData(int ClientId)
        {
            Random rnd = new Random();
            Console.WriteLine("ClientId: " + ClientId);
            byte[] GT100loginData = {
                                         0x78, 0x78,
                                         0x11,
                                         0x01,

                //0x03,
                Convert.ToByte(ClientId.ToString("00"), 16),
                0x03,
                0x03,
                0x03,
                0x03,
                0x03,
                0x03,
                0x03,
                0x03,
                0x03,
                0x03,

                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(ClientId.ToString("00"), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),
                //Convert.ToByte(rnd.Next(1, 99).ToString(), 16),

                                         0xE1, 0x00, 0x01, 0x5D,
                                         0x13,
                                         0x0D, 0x0A
                               };
            byte[] gt100heartBeatData = {
                                         0x78, 0x78, 0x0A, 0x13,
                                         0x44,
                                         0x06,
                                         0x04,
                                         0x00, 0x02,
                                         0x00, 0x07, 0xEF, 0x47, 0x0D, 0x0A
                                     };

            byte[] gt100locationData = {
                                     0x78, 0x78,
                                     0x22,
                                     0x22,
                                     0x10, 0x02, 0x12, 0x0C, 0x22, 0x00,
                                     0xC4,
                                     0x01, 0x2E, 0xE7, 0xF6,
                                     0x08, 0x42, 0xC0, 0x00,
                                     0x00,
                                     0x14, 0x00,
                                     0x01, 0x94, 0x5E, 0x00, 0x4E, 0x00, 0x51, 0x2D, 0x01, 0x05, 0x01,
                                     0x00, 0x08, 0x4B, 0xC0, 0x0D, 0x0A
                                     };

            gt100Data.Add("LoginData", GT100loginData);
            gt100Data.Add("HeartBeatData", gt100heartBeatData);
            gt100Data.Add("LocationData1", gt100locationData);
            gt100Data.Add("LocationData2", gt100locationData);
        }

        public byte[] getData(string dataType)
        {
            if (gt100Data.Keys.Contains(dataType))
            {
                return gt100Data[dataType] as byte[];
            }
            else
                return new byte[0];
        }
    }


    class TR06NewSampleData : ISampleData
    {
        Dictionary<string, object> TR06NewData = new Dictionary<string, object>();

        public TR06NewSampleData(int ClientId)
        {
            Random rnd = new Random();


            byte[] TR06NewloginData = {
                                       0x78, 0x78,
                                        0x0D,0x01,
                                        0x01, 0x23, 0x45, 0x67, 0x89, 0x01, 0x23, 0x45,
                                        0x10 ,0x18,
                                        0x32, 0x00,
                                        0x00, 0x01,
                                        0x8C, 0xDD,
                                        0x0D, 0x0A,
                                      };

            byte[] TR06NewheartBeatData = {
                                            0x78, 0x78,
                                            0x08,
                                            0x13,
                                            0x4B, 0x04, 0x03,
                                            0x00,0x01,
                                            0x00,0x11,
                                            0x06,0x1F,
                                            0x0D,0x0A
                                        };

            byte[] TR06NewlocationData = {
                                    0x78,0x78,
               0x1F, 0x12,
               0x0B, 0x08, 0x1D, 0x11, 0x2E, 0x10,0xCC,0x02,0x7A, 0xC7, 0xEB,
               0x0C,0x46, 0x58, 0x49,0x00,0x14, 0x8F, 0x01, 0xCC, 0x00, 0x28, 0x7D, 0x00, 0x1F, 0xB8, 0x00, 0x03,
               0x80, 0x81, 0x0D, 0x0A,
                                     };

            byte[] TR06NewlocationData1 = {
                                     0x78, 0x78,

                0x1F, 0x12,
               0x0B, 0x08, 0x1D, 0x11, 0x2E, 0x10,0xCC,0x02,0x7A, 0xC7, 0xEB,
               0x0C,0x46, 0x58, 0x49,0x00,0x14, 0x8F, 0x01, 0xCC, 0x00, 0x28, 0x7D, 0x00, 0x1F, 0xB8, 0x00, 0x03,
               0x80, 0x81, 0x0D, 0x0A,
                                     };

            //78781F120D0A

            TR06NewData.Add("LoginData", TR06NewloginData);
            TR06NewData.Add("HeartBeatData", TR06NewheartBeatData);
            TR06NewData.Add("LocationData", TR06NewlocationData);
            TR06NewData.Add("LocationData2", TR06NewlocationData1);
        }

        public byte[] getData(string dataType)
        {
            if (TR06NewData.Keys.Contains(dataType))
            {
                return TR06NewData[dataType] as byte[];
            }
            else
                return new byte[0];
        }
    }
    class GT300SampleData : ISampleData
    {
        Dictionary<string, object> GT300Data = new Dictionary<string, object>();

        public GT300SampleData(int ClientId)
        {
            Random rnd = new Random();

            byte[] GT300loginData = {
                                        0x78, 0x78,
                                        0x0D,
                                        0x01,
                                        0x01, 0x23, 0x45, 0x67, 0x89, 0x01, 0x23, 0x45,
                                        0x10 ,0x18,
                                        0x32, 0x00,
                                        0x00, 0x01,
                                        0x8C, 0xDD,
                                        0x0D, 0x0A,
                               };
            byte[] GT300heartBeatData = {
                                            0x78, 0x78,
                                            0x0A,
                                            0x13,
                                            0x40, 0x06, 0x04,
                                            0x00,0x01,
                                            0x00,0x1F,
                                            0xC4,0x39,
                                            0x0D,0x0A
                                        };

            byte[] GT300locationData = {
                                    0x78,0x78,
               0x1F, 0x22,
               0x0B, 0x08, 0x1D, 0x11, 0x2E, 0x10,0xCF,0x02,0x7A, 0xC7, 0xEB,
               0x0C,0x46, 0x58, 0x49,0x00,0x14, 0x8F, 0x01, 0xCC, 0x00, 0x28, 0x7D, 0x00, 0x1F, 0xB8,0x01,0x01,0x00, 0x00, 0x03,
               0x80, 0x81, 0x0D, 0x0A,
                                     };

            byte[] GT300locationData1 = {
                                     0x78,0x78,
               0x1F, 0x22,
               0x0B, 0x08, 0x1D, 0x11, 0x2E, 0x10,0xCF,0x02,0x7A, 0xC7, 0xEB,
               0x0C,0x46, 0x58, 0x49,0x00,0x14, 0x8F, 0x01, 0xCC, 0x00, 0x28, 0x7D, 0x00, 0x1F, 0xB8,0x01,0x01,0x00, 0x00, 0x03,
               0x80, 0x81, 0x0D, 0x0A,
                                     };


            //78781F120D0A

            GT300Data.Add("LoginData", GT300loginData);
            GT300Data.Add("HeartBeatData", GT300heartBeatData);
            GT300Data.Add("LocationData", GT300locationData);
            GT300Data.Add("LocationData1", GT300locationData1);
            // gt100Data.Add("LocationData2", gt100locationData);
        }

        public byte[] getData(string dataType)
        {
            if (GT300Data.Keys.Contains(dataType))
            {
                return GT300Data[dataType] as byte[];
            }
            else
                return new byte[0];
        }
    }

    class GT03ASampleData : ISampleData
    {
        Dictionary<string, object> GT03AData = new Dictionary<string, object>();

        public GT03ASampleData(int ClientId)
        {
            Random rnd = new Random();

            byte[] GT03AloginData = {
                                        0x78, 0x78,
                                        0x0F,
                                        0x01,
                                        0x01, 0x23, 0x45, 0x67, 0x89, 0x01, 0x23, 0x45,
                                        0x10 ,0x0B,
                                        0x00, 0x01,
                                        0xA3, 0x67,
                                        0x0D, 0x0A,
                               };
            byte[] GT03AheartBeatData = {
                                            0x78, 0x78,
                                            0x0A,
                                            0x13,
                                            0x40, 0x06,0x04,
                                            0x00,0x01,
                                            0x00, 0x1F,
                                            0xC4, 0x39,
                                            0x0D,0x0A
                                        };


            byte[] GT03AlocationData = {
                                    0x78,0x78,
               0x1F, 0x10,
               0x0B, 0x08,0x1D, 0x11, 0x2E, 0x10,0x9C,0x02,0x7A, 0xC7, 0xEB,
               0x0C,0x46, 0x58, 0x49,0x00,0x14, 0x8F,0x00,0x01, 0x00,0x03,
               0x80, 0x81, 0x0D, 0x0A,
                                     };

            byte[] GT03AlocationData1 = {
                                     0x78,0x78,
               0x15, 0x11,
               0x00, 0x00,0x00, 0x00, 0x00, 0x00,
               0x01,0xCC, 0x00, 0x26,
               0x6A,0x00, 0x1D, 0xF1,
               0x00,0x01, 0x00,0x18,
               0x91, 0x88, 0x0D, 0x0A,
                                     };


            //78781F120D0A

            GT03AData.Add("LoginData", GT03AloginData);
            GT03AData.Add("HeartBeatData", GT03AheartBeatData);
            GT03AData.Add("LocationData", GT03AlocationData);
            GT03AData.Add("LocationData1", GT03AlocationData1);
            // gt100Data.Add("LocationData2", gt100locationData);
        }

        public byte[] getData(string dataType)
        {
            if (GT03AData.Keys.Contains(dataType))
            {
                return GT03AData[dataType] as byte[];
            }
            else
                return new byte[0];
        }
    }













}
