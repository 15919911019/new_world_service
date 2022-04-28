using Public.Tools;
using System;
using System.Collections.Generic;
using System.IO;

namespace Business.LocationModels
{
    public static class AreaInfo
    {
        public static List<AreaEntity> Province { get; private set; }

        public static List<AreaEntity> City { get; private set; }

        public static List<AreaEntity> Regional { get; private set; }

        static AreaInfo()
        {
            try
            {
                var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\citys.json";
                if (File.Exists(file) == false)
                    return;

                var contant = File.ReadAllText(file);
                var view = Tool.JsonToObject<List<AreaBaseEntity>>(contant);
                Province = view[0]?.options;
                City = view[1]?.options;
                Regional = view[2]?.options;



                Province?.ForEach(q =>
                {
                    q.letter = Tool.GetSpellCode(q.text);
                });
                City?.ForEach(q =>
                {
                    if (q.text.Equals("亳州市"))
                        q.letter = "BZS";
                    else
                        q.letter = Tool.GetSpellCode(q.text);
                });
                Regional?.ForEach(q =>
                {
                    q.letter = Tool.GetSpellCode(q.text);
                });
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class AreaBaseEntity
    {
        public List<AreaEntity> options { get; set; }
    }

    public class AreaEntity
    {
        public string text { get; set; }
        public string value { get; set; }
        public string parentVal { get; set; }
        public string letter { get; set; }
    }
}
