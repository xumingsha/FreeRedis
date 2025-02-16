﻿using System;
using System.Collections.Generic;

namespace FreeRedis
{
    partial class RedisClient
	{
        public TransactionHook Multi()
        {
            CheckUseTypeOrThrow(UseType.Pooling, UseType.Cluster, UseType.Sentinel, UseType.SingleInside, UseType.SingleTemp);
            return new TransactionHook(this);
        }
        public class TransactionHook : RedisClient
        {
            internal TransactionHook(RedisClient cli) : base(new TransactionAdapter(cli))
            {
                this.Serialize = cli.Serialize;
                this.Deserialize = cli.Deserialize;
            }
            public void Discard() => (Adapter as TransactionAdapter).Discard();
            public object[] Exec() => (Adapter as TransactionAdapter).Exec();
            public void UnWatch() => (Adapter as TransactionAdapter).UnWatch();
            public void Watch(params string[] keys) => (Adapter as TransactionAdapter).Watch(keys);

            ~TransactionHook()
            {
                (Adapter as TransactionAdapter).Dispose();
            }
        }



        // Pipeline
        public PipelineHook StartPipe()
        {
            CheckUseTypeOrThrow(UseType.Pooling, UseType.Cluster, UseType.Sentinel, UseType.SingleInside, UseType.SingleTemp);
            return new PipelineHook(this);
        }
        public class PipelineHook : RedisClient
        {
            internal PipelineHook(RedisClient cli) : base(new PipelineAdapter(cli))
            {
                this.Serialize = cli.Serialize;
                this.Deserialize = cli.Deserialize;
            }
            public object[] EndPipe() => (Adapter as PipelineAdapter).EndPipe();

            ~PipelineHook()
            {
                (Adapter as PipelineAdapter).Dispose();
            }
        }



        // GetShareClient
        public ShareClientHook GetShareClient()
        {
            CheckUseTypeOrThrow(UseType.Pooling, UseType.Sentinel, UseType.SingleInside);
            var rds = Adapter.GetRedisSocket(null);
            return new ShareClientHook(this, new SingleTempAdapter(this, rds, () => rds.Dispose()));
        }
        public class ShareClientHook: RedisClient
        {
            internal ShareClientHook(RedisClient cli, BaseAdapter adapter) : base(adapter)
            {
                this.Serialize = cli.Serialize;
                this.Deserialize = cli.Deserialize;
            }

            ~ShareClientHook()
            {
                (Adapter as SingleTempAdapter).Dispose();
            }
        }
    }
}
