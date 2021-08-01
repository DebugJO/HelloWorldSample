using System;

namespace HttpClientExam.Models
{
    public class WeatherModel
    {
        public DayModel[] Consolidated_weather { get; set; } // = new DayModel[] { new DayModel { } };
        public DateTime Sun_rise { get; set; }
        public DateTime Sun_set { get; set; }
        public string Title { get; set; }
        public string Timezone { get; set; }
    }

    public class DayModel
    {
        public string Weather_state_name { get; set; }
        public string Applicable_date { get; set; }
        public float Min_temp { get; set; }
        public float Max_temp { get; set; }
    }
}
