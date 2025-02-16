﻿using FreeRedis;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;

namespace console_netcore31
{
    class Program
    {
        static Lazy<RedisClient> _cliLazy = new Lazy<RedisClient>(() =>
        {
            //var r = new RedisClient("127.0.0.1:6379", false); //redis 3.2 Single test
            var r = new RedisClient("127.0.0.1:6379,database=1"); //redis 3.2
            //var r = new RedisClient("127.0.0.1:6379,database=1", "127.0.0.1:6379,database=1");
            //var r = new RedisClient("192.168.164.10:6379,database=1"); //redis 6.0
            r.Serialize = obj => JsonConvert.SerializeObject(obj);
            r.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
            //r.Notice += (s, e) => Trace.WriteLine(e.Log);
            return r;
        });
        static RedisClient cli => _cliLazy.Value;

        static void Main(string[] args)
       {
            RedisHelper.Initialization(new CSRedis.CSRedisClient("127.0.0.1:6379,database=2"));
            cli.Set("TestMGet_null1", Class);
            RedisHelper.Set("TestMGet_null1", Class);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            cli.Set("TestMGet_null1", Null);
            cli.Set("TestMGet_string1", String);
            cli.Set("TestMGet_bytes1", Bytes);
            cli.Set("TestMGet_class1", Class);
            cli.Set("TestMGet_null2", Null);
            cli.Set("TestMGet_string2", String);
            cli.Set("TestMGet_bytes2", Bytes);
            cli.Set("TestMGet_class2", Class);
            cli.Set("TestMGet_null3", Null);
            cli.Set("TestMGet_string3", String);
            cli.Set("TestMGet_bytes3", Bytes);
            cli.Set("TestMGet_class3", Class);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");

            sw.Reset();
            sw.Start();
            RedisHelper.Set("TestMGet_null1", Null);
            RedisHelper.Set("TestMGet_string1", String);
            RedisHelper.Set("TestMGet_bytes1", Bytes);
            RedisHelper.Set("TestMGet_class1", Class);
            RedisHelper.Set("TestMGet_null2", Null);
            RedisHelper.Set("TestMGet_string2", String);
            RedisHelper.Set("TestMGet_bytes2", Bytes);
            RedisHelper.Set("TestMGet_class2", Class);
            RedisHelper.Set("TestMGet_null3", Null);
            RedisHelper.Set("TestMGet_string3", String);
            RedisHelper.Set("TestMGet_bytes3", Bytes);
            RedisHelper.Set("TestMGet_class3", Class);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");

        }

        static readonly object Null = null;
        static readonly string String = "我是中国人";
        static readonly byte[] Bytes = Encoding.UTF8.GetBytes("这是一个byte字节");
        static readonly TestClass Class = new TestClass { Id = 1, Name = "Class名称", CreateTime = DateTime.Now, TagId = new[] { 1, 3, 3, 3, 3 } };
    }

    public class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public int[] TagId { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
