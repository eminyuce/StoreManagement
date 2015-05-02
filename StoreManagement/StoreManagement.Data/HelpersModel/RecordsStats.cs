using System;


namespace StoreManagement.Data.HelpersModel
{
    public class RecordsStats
    {
        public int RecordsTotal { get; set; }
        public int RecordFirst { get; set; }
        public int RecordLast { get; set; }
        public int RecordCount { get; set; }

        public override string ToString()
        {
            if (RecordCount > 1)
            {

                return string.Format("{0}-{1} of {2}", RecordFirst,
                    RecordFirst + RecordCount - 1, RecordsTotal);


            }
            else if (RecordCount == 1)
            {
                return string.Format("{0} of {1}", RecordFirst, RecordsTotal);
            }
            else if (RecordCount == 0 && RecordsTotal > 0)
            {
                return string.Format("wrong page {0} records, {1} pages",
                    RecordsTotal, PageLast);
            }
            else
            {
                return string.Format("no records found");
            }

        }



        public int PageCurrent
        {
            get
            {
                if (RecordCount > 0)
                {
                    return RecordLast / (RecordLast - RecordFirst + 1);
                }
                else
                {
                    return 0;
                }

            }
        }




        public int PageLast
        {
            get
            {

                return RecordsTotal / (RecordLast - RecordFirst + 1) + 1;


            }
        }


        private ItemType _ownerType;
        public ItemType OwnerType { get { return _ownerType; } set { _ownerType = value; } }
    }
}
