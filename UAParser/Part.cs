namespace UAParser
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Part
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="family"></param>
        protected Part(string family)
        {
            Family = family;
        }

        /// <summary>
        /// The family, is available
        /// </summary>
        public string Family { get; }
    }
}
