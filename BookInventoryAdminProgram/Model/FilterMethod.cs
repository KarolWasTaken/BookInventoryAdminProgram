using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Model
{
    public class FilterMethod
    {
        public static List<string> MergeSort(List<string> unsortedList)
        {
            if (unsortedList.Count <= 1)
                return unsortedList;

            // split the list up into 2
            int mid = unsortedList.Count / 2;
            List<string> leftHalf = new List<string>(unsortedList.GetRange(0, mid));
            List<string> rightHalf = new List<string>(unsortedList.GetRange(mid, unsortedList.Count - mid));

            // if one of these isn't just 1 element, split it up further.
            // once that has been split up AND sorted, that process finishes
            // and it can resume.
            // if left.count > 1 and right.count <= 1, after left has been sorted
            // resume at line 27.
            leftHalf = MergeSort(leftHalf);
            rightHalf = MergeSort(rightHalf);

            return Merge(leftHalf, rightHalf);
        }

        /// <summary>
        /// Merges two lists alphabetically.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static List<string> Merge(List<string> left, List<string> right)
        {
            List<string> merged = new List<string>();
            int leftIndex = 0;
            int rightIndex = 0;

            // so we dont have an indexOutOfRange error
            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                // using ordinal rules (binary) checks which str goes before which.
                // "< 0" => L goes before R
                // "= 0" => same position (i.e same word)
                // "> 0" => R goes before L
                if (string.Compare(left[leftIndex], right[rightIndex], StringComparison.Ordinal) <= 0)
                {
                    merged.Add(left[leftIndex]); // left goes before right
                    leftIndex++;
                }
                else
                {
                    merged.Add(right[rightIndex]); // right goes before left
                    rightIndex++;
                }
            }

            // This area adds any left overs onto merge
            while (leftIndex < left.Count)
            {
                merged.Add(left[leftIndex]);
                leftIndex++;
            }

            while (rightIndex < right.Count)
            {
                merged.Add(right[rightIndex]);
                rightIndex++;
            }

            return merged;
        }
    }
}
