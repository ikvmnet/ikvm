using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.Sdk.Tasks
{

    /// <summary>
    /// Sorts the input JavaReferenceItem specifications based on their dependencies.
    /// </summary>
    public class IkvmSortJavaReferenceItem : Task
    {

        /// <summary>
        /// Topologically sorts the JavaReferenceItem set.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        static IList<JavaReferenceItem> Sort(IList<JavaReferenceItem> items)
        {
            // construct a map of nodes to their indegrees
            var m = items.ToDictionary(i => i, i => 0);
            foreach (var item in items)
                foreach (var reference in item.References)
                    m[reference]++;

            // track nodes with no incoming edges
            var t = new Queue<JavaReferenceItem>(items.Where(i => m[i] == 0));

            // initially no nodes in our ordering
            var l = new List<JavaReferenceItem>();

            // as long as there are nodes with no incoming edges
            while (t.Count > 0)
            {
                // add one of those nodes to the ordering
                var node = t.Dequeue();
                l.Add(node);

                // decrement the indegree of that node's neightbors
                foreach (var neighbor in node.References)
                {
                    m[neighbor]--;
                    if (m[neighbor] == 0)
                        t.Enqueue(neighbor);
                }
            }

            // we're finished, no cycle
            if (l.Count == items.Count)
                return l.ToArray();

            throw new InvalidOperationException("Cycle detected.");
        }

        /// <summary>
        /// JavaReferenceItem items without assigned hashes.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            Items = Sort(IkvmJavaReferenceItemUtil.Import(Items)).Select(i => i.Item).ToArray();
            return true;
        }

    }

}
