using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BtnInput;

namespace ExtensionMethods
{
    public static class RadixHelper
    {
       public static int ConvertRadixFromTo(RadixOptions source, RadixOptions target, int value)
        {
            int outputValue = 0;
            switch (source)
            {
                case RadixOptions.Binary:
                    {
                        switch (target)
                        {
                            case RadixOptions.Binary:
                                {
                                    //no conversion needed
                                    outputValue = value;
                                }
                                break;
                            case RadixOptions.UnbalancedTernary:
                                {
                                    //value 0 maps to 0 
                                    if (value == 0)
                                        outputValue = 0;
                                    else //value 1 maps to 2
                                        outputValue = 2;
                                }
                                break;
                            case RadixOptions.BalancedTernary:
                                {
                                    //value 0 maps to -1
                                    if (value == 0)
                                        outputValue = -1;
                                    else //value 1 maps to 1
                                        outputValue = 1;
                                }
                                break;
                        }
                    }
                    break;
                case RadixOptions.UnbalancedTernary:
                    {
                        switch (target)
                        {
                            case RadixOptions.Binary:
                                {
                                    //0 and 1 maps to 0
                                    if (value < 2)
                                        outputValue = 0;
                                    else //2 maps to 1
                                        outputValue = 1;
                                }
                                break;
                            case RadixOptions.UnbalancedTernary:
                                {
                                    //no conversion needed
                                    outputValue = value;
                                }
                                break;
                            case RadixOptions.BalancedTernary:
                                {
                                    //map 0 to -1, 1 to 0, 2 to 1 by shifting with value with -1
                                    outputValue = value - 1;
                                }
                                break;

                        }

                    }
                    break;
                case RadixOptions.BalancedTernary:
                    {
                        switch (target)
                        {
                            case RadixOptions.Binary:
                                {
                                    //value -1 and 0 map to 0
                                    if (value < 1)
                                        outputValue = 0;
                                    else //value 1 map to 1
                                        outputValue = 1;
                                }
                                break;
                            case RadixOptions.UnbalancedTernary:
                                {
                                    //map -1 to 0, 0 to 1, 1 to 2 by shifting value + 1
                                    outputValue = value + 1;
                                }
                                break;
                            case RadixOptions.BalancedTernary:
                                {
                                    //no conversion needed
                                    outputValue = value;
                                }
                                break;

                        }

                    }
                    break;                   
            }

            return outputValue;
        }
    }

   
}