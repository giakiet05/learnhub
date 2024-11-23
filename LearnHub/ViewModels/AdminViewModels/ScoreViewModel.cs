using LearnHub.Models;
using LearnHub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class ScoreViewModel : BaseViewModel
    {
        private readonly Score _score;
        public string StudentId => _score.StudentId;
        public double? GKScore => _score.GKScore;
        public double? CKScore => _score.CKScore;
        public string? TXScore => _score.TXScore;
        public double AverageScore => CalculateAverageScore();
        public Subject Subject { get; set; }
        public Student Student { get; set; }
        public AcademicYear AcademicYear { get; set; }

        ScoreViewModel(Score score)
        {
            _score = score;
            Include();
        }
        async void Include()
        {
            Student =  await GenericDataService<Student>.Instance.GetOne(e => e.Id == _score.StudentId);
            Subject = await GenericDataService<Subject>.Instance.GetOne(e=>e.Id == _score.SubjectId);
            AcademicYear = await GenericDataService<AcademicYear>.Instance.GetOne(e=>e.Id == _score.YearId);
        }
        public double  CalculateAverageScore()
        {
            if (string.IsNullOrWhiteSpace(TXScore)) return 0;

            // Tách chuỗi bằng dấu cách và chuyển đổi thành mảng double
            double[] numbers = TXScore.Split(' ')
                                    .Select(s => double.Parse(s.Trim()))
                                    .ToArray();

            // Tính trung bình
            return numbers.Average();
        }
        public static IEnumerable<ScoreViewModel> ConvertToScoreViewModels(IEnumerable<Score> scores)
        {
            return scores.Select(score => new ScoreViewModel(score));
        }
    }
}
