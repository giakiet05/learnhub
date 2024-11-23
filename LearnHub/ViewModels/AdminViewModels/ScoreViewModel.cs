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
        public string? StudentName => _score.Student?.FullName;
        public double? GKScore => _score.GKScore;
        public double? CKScore => _score.CKScore;
        public string? TXScore => _score.TXScore;
        public double AverageScore => CalculateAverageScore();

        ScoreViewModel(Score score)
        {
            _score = score;
            Include();
        }
        async void Include()
        {
           _score.Student =  await GenericDataService<Student>.Instance.GetOne(e => e.Id == StudentId);
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
