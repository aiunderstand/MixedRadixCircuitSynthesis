using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BtnInput;

namespace ExtensionMethods
{
    public static class RadixHelper
    {
        //note that this conversion allows dont care symbols, meaning that value could be 1 higher than radix
       public static int ConvertRadixFromTo(RadixOptions source, RadixOptions target, int value)
        {
            int outputValue = 0;
            switch (source)
            {
                case RadixOptions.Binary:
                case RadixOptions.SignedBinary:
                    {
                        switch (target)
                        {
                            case RadixOptions.Binary:
                            case RadixOptions.SignedBinary:
                                {
                                    //no conversion needed
                                    outputValue = value;
                                }
                                break;
                            case RadixOptions.UnbalancedTernary:
                                {
                                    if (value == 0)
                                        outputValue = 0;

                                    if (value == 1)
                                        outputValue = 2;

                                    if (value == 2) // dont care x
                                        outputValue = 3;
                                }
                                break;
                            case RadixOptions.BalancedTernary:
                                {
                                    //value 0 maps to -1
                                    if (value == 0)
                                        outputValue = -1;

                                    if (value == 1)
                                        outputValue = 1;

                                    if (value == 2) // dont care x
                                        outputValue = 2;
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
                            case RadixOptions.SignedBinary:
                                {
                                    //0 maps to 0
                                    if ((value == 0))
                                        outputValue = 0;

                                    //1 and 2 maps to 1
                                    if (value == 1 || value == 2)
                                        outputValue = 1;

                                    //dont care maps to dont care
                                    if (value == 3)
                                        outputValue = 2;
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
                            case RadixOptions.SignedBinary:
                                {
                                    //-1 maps to 0
                                    if (value == -1)
                                        outputValue = 0;

                                    //0 and 1 maps to 1
                                    if (value == 0 || value == 1)
                                        outputValue = 1;

                                    //dont care maps to dont care
                                    if (value == 2)
                                        outputValue = 2;
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
                case RadixOptions.Unknown: //eg. often used for the initial condition, trying to fit the source into the target, so sanitizing input to min or max values of target radix, incl. dont care
                    {
                        switch (target)
                        {
                            case RadixOptions.Binary:
                            case RadixOptions.SignedBinary:
                                {
                                    if (value <= 0)
                                        outputValue = 0;
                                    if (value == 1)
                                        outputValue = 1;
                                    if (value > 1) //dont care
                                        outputValue = 2;
                                }
                                break;
                            case RadixOptions.UnbalancedTernary:
                                {
                                    if (value <= 0)
                                        outputValue = 0;

                                    if (value == 1)
                                        outputValue = 1;

                                    if (value == 2)
                                        outputValue = 2;

                                    if (value > 2) //dont care
                                        outputValue = 3;
                                }
                                break;
                            case RadixOptions.BalancedTernary:
                                {
                                     if (value <= -1)
                                        outputValue = -1;

                                    if (value == 0)
                                        outputValue = 0;

                                    if (value == 1)
                                        outputValue = 1;

                                    if (value > 1) //dont care
                                        outputValue = 2;
                                }
                                break;
                        }
                    }
                    break;
            }

            return outputValue;
        }

       public static int GetValueAsIndex(RadixOptions radixSource,int value)
        {
            int outputValue = 0;

            switch (radixSource)
                        {
                            case RadixOptions.Binary:
                            case RadixOptions.SignedBinary:
                                {
                                    outputValue = value;
                                }
                                break;
                            case RadixOptions.UnbalancedTernary:
                                {
                                    outputValue = value;
                                }
                                break;
                            case RadixOptions.BalancedTernary:
                                {
                                    outputValue = value + 1;
                                }
                                break;
                        }

            return outputValue;
        }

    }


}
