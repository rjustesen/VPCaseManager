using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager
{
    public class ViewHelper
    {
        public static bool ColumnInView(string ColumnKey, string IndexString)
        {
            try
            {
                string lsTemp = IndexString;

                while (lsTemp.Length > 0)
                {
                    int liEnd = lsTemp.IndexOf(',');

                    if (liEnd > -1)
                    {
                        string lsIndex = lsTemp.Substring(0, liEnd);
                        if (ColumnKey == lsIndex)
                        {
                            return true;
                        }
                        lsTemp = lsTemp.Substring(liEnd + 1);
                    }
                    else
                    {
                        // Reached the end of the string
                        break;
                    }
                }
            }
            catch
            {
                // Ignore errors and continue
            }

            return false;
        }
    
        public static bool IndexInView(int index, string indexString)
        {
            try
            {
                string lsTemp = indexString;

                while (lsTemp.Length > 0)
                {
                    int liEnd = lsTemp.IndexOf(',');

                    if (liEnd > -1)
                    {
                        string lsIndex = lsTemp.Substring(0, liEnd);
                        if (int.TryParse(lsIndex, out int parsedIndex) && parsedIndex == index)
                        {
                            return true;
                        }
                        lsTemp = lsTemp.Substring(liEnd + 1);
                    }
                    else
                    {
                        // No more commas, end of string reached
                        break;
                    }
                }
            }
            catch
            {
                // Ignore errors and continue
            }

            return false;
        }
    }
}
