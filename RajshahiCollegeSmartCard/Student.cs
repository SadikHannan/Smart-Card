using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RajshahiCollegeSmartCard
{
   public class Student
    {
        public String name { get; set; }
        public String b64_image { get; set; }

        public string dept_name { get; set; }

        public string session { get; set; }


		public string status { get; set; }

		public string current_level { get; set; }

		public string registration_status { get; set; }

		public string father_name { get; set; }

		public string mother_name { get; set; }

		public string birth_date { get; set; }

		public string gender { get; set; }

		public string faculty_name { get; set; }

		public string contact_no { get; set; }

		public string religion { get; set; }


	}

    public class ESMStudent
    {
        public String academic_year { get; set; }
        public String student_id { get; set; }

        public string StudentName { get; set; }

        public string Class { get; set; }


        public string Department { get; set; }

        public string Shift { get; set; }

        public string section { get; set; }

        public string roll { get; set; }

        public string photo { get; set; }

        public string father_name { get; set; }

        public string mother_name { get; set; }

        public string guardian_mobile { get; set; }

        public string b64_image { get; set; }

        public string birth_date { get; set; }

        public string gender { get; set; }

        public string religion { get; set; }

    }


}
