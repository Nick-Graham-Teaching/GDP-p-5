using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public class Parent
    {
        public Parent()
        {
            Debug.Log("Parent Cons");
        }
        //public Vector3 ParenVec;

        public Transform tr;

        public Parent(Transform tr)
        {
            //ParenVec = new(vec);
            this.tr = tr;
        }
        public virtual void Print()
        {
            Debug.Log("Parent");
        }
    }

    public class Child : Parent
    {
        public Coroutine testCoroutine;
        public Child()
        {
            Debug.Log("Child Cons");
            testCoroutine = TestFile.Instance.StartCoroutine(Test());
        }
        public IEnumerator Test()
        {
            while (true)
            {
                Debug.Log("Child Coroutine");
                yield return new WaitForSeconds(0.5f);
            }
        }
        public override void Print()
        {
            Debug.Log("Child ");
        }
    }

    public class Grandchild : Child
    {
        public Grandchild()
        {
            Debug.Log("Grandchild Cons");
        }
        public override void Print()
        {
            Debug.Log("Grandchild");
        }
    }

    public class TestFile : Singleton<TestFile>
    {
        [SerializeField]
        protected int a;

        private Child p;

        Vector3 vec;

        // Start is called before the first frame update
        void Start()
        {
            vec = new(1, 1, 1);
            p = new Child();
            //Debug.Log(transform.position);
            //transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            //Debug.Log(p.tr.position);
            //Debug.Log(transform.position);
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            yield return new WaitForSeconds(5.0f);
            Debug.Log("here");
            StopCoroutine(p.testCoroutine);
        }
    }
}

