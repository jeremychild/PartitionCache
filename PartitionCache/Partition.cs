﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartitionCache
{
    /// <summary>
    /// Partition. A container / silo of producers
    /// </summary>
    public class Partition
    {

        public DateTime LastCleanedUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int Number { get; set; }
        public long Throughput { get; set; }
        public Partition()
        {
            Producers = new ConcurrentDictionary<string, Producer>();
        }
        public ConcurrentDictionary<string, Producer> Producers { get; set; }
        public int ProducerCount
        {
            get
            {
                try
                {
                    return Producers.Count;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public void IncreaseThroughput()
        {
            try
            {
                lock (this)
                {
                    Throughput += 1;
                }
            }
            catch (Exception)
            {
                try
                {
                    if (Throughput != Int64.MaxValue)
                    {
                        Throughput = (Throughput + 1);
                    }
                }
                catch (Exception)
                {
                    Throughput = Int64.MaxValue;
                }
            }
        }

    }

}
