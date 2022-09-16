using ENI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENI.Controller
{
    public class ReportInsertionsController
    {
        public static List<reportInsertionsDTO> List()
        {
            List<reportInsertionsDTO> list = new List<reportInsertionsDTO>();
            eniEntities db = new eniEntities();

            var reportList = db.report.ToList();

            for (int i = 0; i < reportList.Count; i++)
            {
                var _report = reportList[i];
                var reportInsertionsList = db.report_insertions.Where(o => o.report_id == _report.id).ToList();

                reportInsertionsDTO insertionItem = new reportInsertionsDTO
                {
                    id = _report.id,
                    display_id = _report.display_id,
                    display_location = _report.display_location,
                    display_name = _report.display_name,
                    display_orientation = _report.display_orientation,
                    display_size = _report.display_size,
                    display_token = _report.display_token,
                    first_created_date = _report.created_date,
                    media_id = _report.media_id,
                    media_name = _report.media_name,
                    insertions = reportInsertionsList
                };

                list.Add(insertionItem);
            }


            return list;
        }
    }
}