using System;

namespace QRSpace.Server.Services
{
    public class UserIdGenerator : IUserIdGenerator
    {
        private const long WorkerId = 1;
        private const long Twepoch = 687888001020L; //唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳
        private static long _sequence;
        private const int WorkerIdBits = 4; //机器码字节数。4个字节用来保存机器码(定义为Long类型会出现，最大偏移64位，所以左移64位没有意义)

        //private const long maxWorkerId = -1L ^ -1L << workerIdBits; //最大机器ID
        private const int SequenceBits = 10; //计数器字节数，10个字节用来保存计数码

        private const int WorkerIdShift = SequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits; //时间戳左移动位数就是机器码和计数器总字节数
        private const long SequenceMask = -1L ^ -1L << SequenceBits; //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        private long _lastTimestamp = -1L;

        public ulong NextId()
        {
            lock (this)
            {
                var timestamp = TimeGen();
                if (_lastTimestamp == timestamp)
                { //同一微妙中生成ID
                    _sequence = (_sequence + 1) & SequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (_sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = TillNextMillis(_lastTimestamp);
                    }
                }
                else
                { //不同微秒生成ID
                    _sequence = 0; //计数清0
                }
                if (timestamp < _lastTimestamp)
                { //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                    throw new Exception(
                        $"Clock moved backwards.  Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
                }
                _lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                var nextId = (timestamp - Twepoch << TimestampLeftShift) | WorkerId << WorkerIdShift | _sequence;
                return (ulong)nextId;
            }
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private static long TillNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns></returns>
        private static long TimeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}