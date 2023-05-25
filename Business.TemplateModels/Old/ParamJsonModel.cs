using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels.Old
{
    public class ParamJsonModel
    {
        public List<TemplateValueModel> Level1 { get; set; } = new List<TemplateValueModel>();

        public List<TemplateValueModel> Level2 { get; set; } = new List<TemplateValueModel>();

        public List<TemplateValueModel> Level3 { get; set; } = new List<TemplateValueModel>();

        public List<TemplateValueModel> Level4 { get; set; } = new List<TemplateValueModel>();

        public string ToJson()
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("{");

            int idx1 = 0;
            Level1.ForEach(q =>
            {
                idx1++;
                var where2 = Level2.Where(f => q.RecordID == f.ParentID).ToList();
                if (where2.Count == 0)
                {
                    if (idx1 < Level1.Count)
                        sb1.AppendLine(GetRowJson(q.Name, q.Value));
                    else
                        sb1.AppendLine(GetRowJson(q.Name, q.Value).TrimEnd(','));
                    return;
                }

                StringBuilder sb2 = new StringBuilder();
                sb2.Append($"\"{q.Name}\":").AppendLine("{");
                int idx2 = 0;
                where2.ForEach(f =>
                {
                    idx2++;
                    var where3 = Level3.Where(d => d.ParentID == f.RecordID).ToList();
                    if (where3.Count == 0)
                    {
                        if (idx2 < where2.Count)
                            sb2.AppendLine(GetRowJson(f.Name, f.Value));
                        else
                            sb2.AppendLine(GetRowJson(f.Name, f.Value).TrimEnd(','));
                        return;
                    }
                    StringBuilder sb3 = new StringBuilder();
                    sb3.Append($"\"{f.Name}\":").AppendLine("{");
                    int idx3 = 0;
                    where3.ForEach(w =>
                    {
                        idx3++;
                        var where4 = Level4.Where(s => s.ParentID == w.RecordID).ToList();
                        if (where4.Count != 0)
                        {
                            StringBuilder sb4 = new StringBuilder();
                            int idx4 = 0;
                            sb4.Append($"\"{w.Name}\":").AppendLine("{");
                            where4.ForEach(d =>
                            {
                                idx4++;
                                if (idx4 < where4.Count)
                                    sb4.AppendLine(GetRowJson(d.Name, d.Value));
                                else
                                    sb4.AppendLine(GetRowJson(d.Name, d.Value).TrimEnd(','));
                            });
                            sb4.AppendLine("}");
                            sb3.Append(sb4.ToString());
                        }
                        else
                        {
                            var str = GetRowJson(w.Name, w.Value);
                            if (idx3 < where3.Count)
                                sb3.AppendLine(str);
                            else
                                sb3.AppendLine(str.TrimEnd(','));
                        }
                    });
                    if (idx2 < where2.Count & sb3.Length > 0)
                    {
                        var temp2 = sb3.ToString().TrimEnd(',') + "},";
                        sb2.Append(temp2);
                    }
                    else if (sb3.Length > 0)
                    {
                        var temp2 = sb3.ToString().TrimEnd(',') + "}";
                        sb2.Append(temp2);
                    }
                });

                if (idx1 < Level1.Count & sb2.Length > 0)
                {
                    var temp3 = sb2.ToString().TrimEnd(',') + "},";
                    sb1.AppendLine(temp3);
                }
                else if (sb2.Length > 0)
                {
                    var temp3 = sb2.ToString().TrimEnd(',') + "}";
                    sb1.AppendLine(temp3);
                }

                idx1++;
            });
            sb1.AppendLine("}");

            var res = sb1.ToString();
            return res;
        }

        public string GetRowJson(string key, string value)
        {
            StringBuilder sb = new StringBuilder();
            var bol = double.TryParse(value, out var val);
            if (value.Contains(",") == true)
            {
                var arr = value.Split(',').ToList();
                StringBuilder sb1 = new StringBuilder();
                arr.ForEach(q => sb1.Append(q).Append(","));
                sb.Append($"\"{key}\" : [{sb1.ToString().TrimEnd(',')}]");
            }
            else if (bol == true)
                sb.Append($"\"{key}\" : {value}");
            else
                sb.Append($"\"{key}\" : \"{value}\"");

            sb.Append(",");
            return sb.ToString();
        }
    }
}
