using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent {
    public Parent() { Debug.Log("Parent Cons"); }
    public IEnumerator Test(Parent a) {
        yield return new WaitForSeconds(1.0f);
        a = new Child();
    }
    public virtual void Print() {
        Debug.Log("Parent" );

    }
}

public class Child : Parent
{
    public Child(){ Debug.Log("Child Cons"); }
    public override void Print()
    {
        Debug.Log("Child ");
    }
}

public class Grandchild : Child
{
    public Grandchild() { Debug.Log("Grandchild Cons");  }
    public override void Print()
    {
        Debug.Log("Grandchild");
    }
}

public class TestFile : Singleton<TestFile>
{
    [SerializeField]
    protected int a;

    private Parent p;
    protected void UpdateState()
    {
        Debug.Log("TestUpdate");
    }
    // Start is called before the first frame update
    void Start()
    {
        p = new Parent();
        StartCoroutine(p.Test(p));
        p = new Child();
        //Debug.Log("--------------------------------------");
        //p.Print();
    }
}
