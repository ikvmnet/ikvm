﻿extern alias helloworld2;

using System;
using System.Collections.Generic;
using System.Text;

namespace IKVM.MSBuild.Tests.Project.Lib
{

    public static class Helloworld
    {

        public static string SayHello(string value)
        {
            return new sample.HelloworldImpl().sayHello(value);
        }

    }

}
