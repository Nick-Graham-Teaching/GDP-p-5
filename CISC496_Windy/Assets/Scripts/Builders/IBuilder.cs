using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{
    public interface IBuilder<T> where T : IBehaviour
    {
        public abstract T Build();
    }

}