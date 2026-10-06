using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Execution;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Compares indented text renderings line by line.
    /// </summary>
    static class TextDiff
    {

        const int MaxHunks = 200;

        /// <summary>
        /// Asserts that two renderings are equal, reporting a line diff where each hunk is preceded by the less-indented
        /// lines that enclose it.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="because"></param>
        public static void ShouldMatch(string actual, string expected, string because)
        {
            if (actual == expected)
                return;

            var a = expected.Split('\n');
            var b = actual.Split('\n');
            var edits = Diff(a, b);
            var report = new StringBuilder();
            var hunks = 0;

            for (int i = 0; i < edits.Count && hunks < MaxHunks; i++)
            {
                if (edits[i].Kind == ' ')
                    continue;

                var lines = edits[i].Kind == '-' ? a : b;
                report.Append("@@ ").Append(edits[i].Kind == '-' ? "expected" : "actual").Append(" line ").Append(edits[i].Index + 1).Append('\n');
                foreach (var h in Headers(lines, edits[i].Index))
                    report.Append("   ").Append(h).Append('\n');

                for (; i < edits.Count && edits[i].Kind != ' '; i++)
                    report.Append(edits[i].Kind).Append("  ").Append(edits[i].Kind == '-' ? a[edits[i].Index] : b[edits[i].Index]).Append('\n');

                hunks++;
            }

            var changed = edits.Count(e => e.Kind != ' ');
            throw new AssertionFailedException($"Renderings differ {because} ({changed} changed lines; '-' expected, '+' actual):\n{report}");
        }

        /// <summary>
        /// Gets the less-indented lines enclosing the given line, outermost first.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static List<string> Headers(string[] lines, int index)
        {
            var result = new List<string>();
            var indent = Indent(lines[index]);
            for (int i = index - 1; i >= 0 && indent > 0; i--)
            {
                var j = Indent(lines[i]);
                if (j < indent)
                {
                    result.Add(lines[i]);
                    indent = j;
                }
            }

            result.Reverse();
            return result;
        }

        static int Indent(string line)
        {
            var i = 0;
            while (i < line.Length && line[i] == ' ')
                i++;

            return i;
        }

        /// <summary>
        /// Computes a shortest line edit script with the Myers algorithm. Unchanged and removed lines index into
        /// <paramref name="a"/>; inserted lines index into <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static List<(char Kind, int Index)> Diff(string[] a, string[] b)
        {
            int n = a.Length, m = b.Length, max = n + m;
            var v = new int[2 * max + 2];
            var trace = new List<int[]>();

            for (int d = 0; d <= max; d++)
            {
                trace.Add((int[])v.Clone());
                for (int k = -d; k <= d; k += 2)
                {
                    var x = k == -d || (k != d && v[max + k - 1] < v[max + k + 1]) ? v[max + k + 1] : v[max + k - 1] + 1;
                    var y = x - k;
                    while (x < n && y < m && a[x] == b[y])
                    {
                        x++;
                        y++;
                    }

                    v[max + k] = x;
                    if (x >= n && y >= m)
                        return Backtrack(trace, n, m, max);
                }
            }

            throw new InvalidOperationException("Unreachable.");
        }

        static List<(char Kind, int Index)> Backtrack(List<int[]> trace, int n, int m, int max)
        {
            var result = new List<(char Kind, int Index)>();
            int x = n, y = m;

            for (int d = trace.Count - 1; d >= 0; d--)
            {
                var v = trace[d];
                var k = x - y;
                var pk = k == -d || (k != d && v[max + k - 1] < v[max + k + 1]) ? k + 1 : k - 1;
                var px = v[max + pk];
                var py = px - pk;

                while (x > px && y > py)
                {
                    x--;
                    y--;
                    result.Add((' ', x));
                }

                if (d > 0)
                {
                    if (x == px)
                        result.Add(('+', --y));
                    else
                        result.Add(('-', --x));
                }
            }

            result.Reverse();
            return result;
        }

    }

}
