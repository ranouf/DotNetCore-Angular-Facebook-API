using System;
using System.Collections.Generic;
using System.Text;
using PrintMyLife.Core.Sample;

namespace PrintMyLife.Infrastructure.Sample
{
  public class SampleService : ISampleService
  {
    public string SayHello()
    {
      return "Hello";
    }
  }
}
