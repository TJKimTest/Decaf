using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.Manager
{
    public class GlobalDataContainer
    {
        public int RecommendedAmountCaffeineValue { get; private set; }
        public void SetRecoommendedAmmoutCaffeine(int caffeineValue)
            => RecommendedAmountCaffeineValue = caffeineValue;
    }

}
