using UnityEngine;

namespace OmNomNomtek.Services
{
  public class SampleDependency : ISampleDependency
  {
    public void DoSomething()
    {
      Debug.Log("SampleDependency.DoSomething()");
    }
  }
}
