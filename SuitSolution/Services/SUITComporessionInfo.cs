using System;
using System.Collections;
using System.Collections.Generic;
using SuitSolution.Services;

public class SUITCompressionInfo : SUITKeyMap<int>
{
    private Dictionary<string, int> compressionMethods = new Dictionary<string, int>();

    public SUITCompressionInfo() : base()
    {
        InitializeCompressionMethods();

    }

    

        private void InitializeCompressionMethods()
        {
            compressionMethods.Add("gzip", 1);
            compressionMethods.Add("bzip2", 2);
            compressionMethods.Add("deflate", 3);
            compressionMethods.Add("lz4", 4);
            compressionMethods.Add("lzma", 7);
        }

        public void AddCompressionMethod(string methodName, int methodValue)
        {
            compressionMethods[methodName] = methodValue;
        }

        public bool TryGetCompressionMethod(string methodName, out int methodValue)
        {
            return compressionMethods.TryGetValue(methodName, out methodValue);
        }
    }

  


    
