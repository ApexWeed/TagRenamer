using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;

namespace Tag_Renamer
{
    public partial class MainForm : Form
    {
        private class TagFormat
        {
            public string Tag;
            public string Description;
            public string Field;
            public int FormatIndex;

            public TagFormat(string Tag, string Description, string Field, int FormatIndex)
            {
                this.Tag = Tag;
                this.Description = Description;
                this.Field = Field;
                this.FormatIndex = FormatIndex;
            }
        }

        private static List<TagFormat> TagFormats;
        private static List<string> Extensions;
        private List<string> InputFiles;
        private List<string> OutputFiles;

        public MainForm()
        {
            InitializeComponent();

            var ci = CultureInfo.InstalledUICulture;
            if (TagFormats == null)
            {
                if (ci.Name == "ja-JP")
                {
                    TagFormats = new List<TagFormat>(new TagFormat[]
                        {
                            new TagFormat("t", "タイトル", "Title", 0),
                            new TagFormat("p", "パフォーマー", "$Performers", 1),
                            new TagFormat("pf", "第1パフォーマ", "FirstPerformer", 2),
                            new TagFormat("a", "アルバムアーティスト", "$AlbumArtists", 3),
                            new TagFormat("af", "第1アルバムアーティスト", "FirstAlbumArtist", 4),
                            new TagFormat("c", "コンポーザー", "$Composers", 5),
                            new TagFormat("cf", "第1コンポーザー", "FirstComposer", 6),
                            new TagFormat("A", "アルバム", "Album", 7),
                            new TagFormat("cmt", "コメント", "Comment", 8),
                            new TagFormat("g", "ジャンル", "$Genres", 9),
                            new TagFormat("gf", "第1ジャンル", "FirstGenre", 10),
                            new TagFormat("y", "年", "Year", 11),
                            new TagFormat("T", "トラック", "Track", 12),
                            new TagFormat("Tc", "トラックカウント", "TrackCount", 13),
                            new TagFormat("d", "ディスク", "Disc", 14),
                            new TagFormat("dc", "ディスクカウント", "DiscCount", 15),
                            new TagFormat("l", "歌詞", "Lyrics", 16),
                            new TagFormat("G", "グルーピング", "Grouping", 17),
                            new TagFormat("bpm", "BPM", "BeatsPerMinute", 18),
                            new TagFormat("C", "コンダクター", "Conductor", 19),
                            new TagFormat("Co", "コピーライト", "Copyright", 20)
                        });
                }
                else
                {
                    TagFormats = new List<TagFormat>(new TagFormat[]
                        {
                            new TagFormat("t", "Title", "Title", 0),
                            new TagFormat("p", "Performer", "$Performers", 1),
                            new TagFormat("pf", "First Performer", "FirstPerformer", 2),
                            new TagFormat("a", "Album Artist", "$AlbumArtists", 3),
                            new TagFormat("af", "First Album Artist", "FirstAlbumArtist", 4),
                            new TagFormat("c", "Composer", "$Composers", 5),
                            new TagFormat("cf", "First Composer", "FirstComposer", 6),
                            new TagFormat("A", "Album", "Album", 7),
                            new TagFormat("cmt", "Comment", "Comment", 8),
                            new TagFormat("g", "Genre", "$Genres", 9),
                            new TagFormat("gf", "First Genre", "FirstGenre", 10),
                            new TagFormat("y", "Year", "Year", 11),
                            new TagFormat("T", "Track Number", "Track", 12),
                            new TagFormat("Tc", "Track Count", "TrackCount", 13),
                            new TagFormat("d", "Disk", "Disc", 14),
                            new TagFormat("dc", "Disk Count", "DiscCount", 15),
                            new TagFormat("l", "Lyrics", "Lyrics", 16),
                            new TagFormat("G", "Grouping", "Grouping", 17),
                            new TagFormat("bpm", "BPM", "BeatsPerMinute", 18),
                            new TagFormat("C", "Conductor", "Conductor", 19),
                            new TagFormat("Co", "Copyright", "Copyright", 20)
                        });
                }
            }

            if (ci.Name != "ja-JP")
            {
                lblFormat.Text = "Format:";
                btnRun.Text = "Run";
                btnClear.Text = "Clear";
                colInput.Text = "Input";
                colOutput.Text = "Output";
                this.Text = "Tag Renamer";
            }

            if (Extensions == null)
            {
                Extensions = new List<string>(new string[]
                {
                    ".mp3",
                    ".flac",
                    ".alac",
                    ".tak",
                    ".wav",
                    ".tta",
                    ".wma",
                    ".ogg",
                    ".m4a",
                });
            }
            InputFiles = new List<string>();
            OutputFiles = new List<string>();

            var sb = new StringBuilder();
            foreach (var tagFormat in TagFormats)
            {
                sb.Append($"*{tagFormat.Tag}* - {tagFormat.Description}, ");
            }
            lblTag.Text = sb.ToString().Substring(0, sb.ToString().Length - 2);
        }

        private void UpdateDisplay()
        {
            lstItems.Items.Clear();
            string[] item = new string[2];
            for (int i = 0; i < InputFiles.Count; i++)
            {
                item[0] = Path.GetFileNameWithoutExtension(InputFiles[i]);
                item[1] = Path.GetFileNameWithoutExtension(OutputFiles[i]);
                lstItems.Items.Add(new ListViewItem(item));
            }
        }

        private async void txtFormat_TextChanged(object sender, EventArgs e)
        {
            await ProcessTags();
        }

        private async Task ProcessTags()
        {
            if (InputFiles.Count > 0)
            {
                var usedTags = new List<int>();
                var format = txtFormat.Text;
                // Convert from human readable to string.format spec.
                foreach (var tagFormat in TagFormats)
                {
                    format = format.Replace($"*{tagFormat.Tag}*", $"{{{tagFormat.FormatIndex}}}");
                    if (format.Contains($"{{{tagFormat.FormatIndex}}}"))
                    {
                        usedTags.Add(tagFormat.FormatIndex);
                    }
                }

                // Also handle custom formatting commands.
                /*
                string[] parseResult;
                while (format.TryParseExact("%{0}!{1}%", out parseResult, false))
                {
                    var section = $"%{parseResult[0]}!{parseResult[1]}%";
                    var tagFormat = TagFormats.Find((x) => x.Tag == parseResult[0]);
                    format = format.Replace(section, $"{{{tagFormat.FormatIndex}:{parseResult[1]}}}");
                    usedTags.Add(tagFormat.FormatIndex);
                }
                */

                var asteriskCount = format.ToCharArray().Count((x) => x == '*');
                while (asteriskCount % 2 == 0 && asteriskCount > 1)
                {
                    var startIndex = format.IndexOf('*');
                    var endIndex = format.IndexOf('*', startIndex + 1);
                    // *<id>!<format>*
                    var tag = format.Substring(startIndex, endIndex - startIndex + 1);
                    // <id>!<format>
                    var parts = tag.Substring(1, tag.Length - 2).Split('!');
                    var outputTag = "";
                    if (parts.Length == 2)
                    {
                        var tagFormat = TagFormats.Find((x) => x.Tag == parts[0]);
                        // {<id>:<format>}
                        outputTag = $"{{{tagFormat.FormatIndex}:{parts[1]}}}";
                        usedTags.Add(tagFormat.FormatIndex);
                    }
                    format = format.Replace(tag, outputTag);
                    asteriskCount = format.ToCharArray().Count((x) => x == '*');
                }

                OutputFiles.Clear();
                await Task.Run(() =>
                {
                    var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
                    for (int i = 0; i < InputFiles.Count; i++)
                    {
                        var tagValues = new object[TagFormats.Count];
                        var tags = TagLib.File.Create(InputFiles[i]);

                        // Reflect onto the tag values.
                        foreach (var tagFormat in TagFormats)
                        {
                            if (tagFormat.Field.StartsWith("$"))
                            {
                                var subTagProperty = tags.GetType().GetProperty("Tag", bindFlags);
                                var subTag = subTagProperty.GetValue(tags);
                                var property = subTag.GetType().GetProperty(tagFormat.Field.Substring(1), bindFlags);

                                var value = (string[])property.GetValue(subTag);
                                tagValues[tagFormat.FormatIndex] = value.Implode();
                            }
                            else
                            {
                                var subTagProperty = tags.GetType().GetProperty("Tag", bindFlags);
                                var subTag = subTagProperty.GetValue(tags);
                                var property = subTag.GetType().GetProperty(tagFormat.Field, bindFlags);

                                var value = property.GetValue(subTag);
                                tagValues[tagFormat.FormatIndex] = value;
                            }
                        }

                        var outputPath = Path.Combine(Path.GetDirectoryName(InputFiles[i]), FilenameString($"{string.Format(format, tagValues)}{Path.GetExtension(InputFiles[i])}"));
                        // Cut the end off the longest tag if it goes over the WIN32 limits. .NET using the proper WChar WIN32 API functions when?
                        if (outputPath.Length > 259)
                        {
                            var longestTag = 0;
                            var longestTagIdx = 0;
                            foreach (var tag in usedTags)
                            {
                                if (tagValues[tag].ToString().Length > longestTag)
                                {
                                    longestTag = tagValues[tag].ToString().Length;
                                    longestTagIdx = tag;
                                }
                            }

                            // Reduce tag length by how far over 259 we are plus 1 for ellipsis.
                            tagValues[longestTagIdx] = string.Concat(tagValues[longestTagIdx].ToString().Substring(0, longestTag - (outputPath.Length - 259) - 1), "…");
                            outputPath = Path.Combine(Path.GetDirectoryName(InputFiles[i]), FilenameString($"{string.Format(format, tagValues)}{Path.GetExtension(InputFiles[i])}"));
                        }
                        OutputFiles.Add(outputPath);
                    }
                });

                UpdateDisplay();
            }
        }

        static char[] illegalChars = Path.GetInvalidFileNameChars();
        static string illegalPattern = string.Format("[{0}]", Regex.Escape(string.Join("", illegalChars)));
        private string FilenameString(string Input)
        {
            var output = Regex.Replace(Input, illegalPattern, "");
            return output;
        }

        private List<string> GetFiles(string[] Paths)
        {
            var files = new List<string>();

            foreach (var path in Paths)
            {
                files.AddRange(GetFiles(path));
            }

            return files;
        }

        private List<string> GetFiles(string Path)
        {
            if (Directory.Exists(Path))
            {
                var files = new List<string>();
                foreach (var file in Directory.GetFiles(Path))
                {
                    files.Add(file);
                }
                foreach (var directory in Directory.GetDirectories(Path))
                {
                    files.AddRange(GetFiles(directory));
                }

                return files;
            }
            else
            {
                return new List<string>(new string[] { Path });
            }
        }

        private async void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var drop = (string[])e.Data.GetData(DataFormats.FileDrop);
            var files = GetFiles(drop);
            foreach (var file in files)
            {
                if (Extensions.Contains(Path.GetExtension(file)))
                {
                    InputFiles.Add(file);
                }
            }
            await ProcessTags();
            UpdateDisplay();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < InputFiles.Count; i++)
            {
                System.IO.File.Move(InputFiles[i], OutputFiles[i]);
            }
            InputFiles.Clear();
            UpdateDisplay();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            InputFiles.Clear();
            UpdateDisplay();
        }
    }
}
