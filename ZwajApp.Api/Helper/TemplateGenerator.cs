using System.Text;
using AutoMapper;
using ZwajApp.Api.Data;
using ZwajApp.Api.DTOS;

namespace ZwajApp.Api.Helper
{
    public class TemplateGenerator
    {
        	private readonly IMapper _mapper;
        private readonly IZwajRepository _repo;

        public TemplateGenerator(IZwajRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        public string GetHTMLStringForUser(int userId)
        {
			//exception of Global Query Filter we use false
            var user = _repo.GetUser(userId).Result;
            var userToReturn = _mapper.Map<UserDetailsDto>(user);

            var likers = _repo.GetLikersOrLikees(userId, "likers").Result;
            var likees = _repo.GetLikersOrLikees(userId, "likees").Result;
            var likersCount=likers.Count;
            var likeesCount=likees.Count;


            var sb = new StringBuilder();

            sb.Append(@"
                        <html dir='ltr'>
                            <head>
                            </head>
                            <body>
                                <div class='page-header'><h2 class='header-container'>Card " + userToReturn.KownAs+ @"</h2></div>
                                                             
                                <div class='card-data'>
                                 <img src='" + userToReturn.PhotoUrl+ @"'>
                                <table style='display:inline;width: 50%;height: 300px;'>
                                <div>
                                <tr>
                                <td>Name</td>
                                    <td>" + userToReturn.KownAs + @"</td>
                                </tr>
                                <tr>
                                    <td>Age</td>
                                    <td>" + userToReturn.Age + @"</td>
                                </tr>    
                                <tr>
                                    <td>Country</td>
                                    <td>" + userToReturn.Country + @"</td>
                                </tr>    
                                <tr>
                                    <td>Membership Since</td>
                                    <td>" + userToReturn.Created + @"</td>
                                </tr> 
                                </div>   
                              </table>
                                </div>
                                <div class='page-header'><h2 class='header-container'>Likers &nbsp;&nbsp;["+likersCount+@"]</h2></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <th>Membership Since</th>
                                        <th>Age</th>
                                        <th>Country</th>
                                    </tr>");

            foreach (var liker in likers)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", liker.KownAs, liker.Created.ToShortDateString(), liker.DateOfBirth.CalculateAge(), liker.Country);
            }

            sb.Append(@"
                                </table>
                                <div class='page-header'><h2 class='header-container'>Likees &nbsp;&nbsp;["+likeesCount+@"] </h2></div>
                                <table align='center'>
                                <tr>
                                 <th>Name</th>
                                        <th>Membership Since</th>
                                        <th>Age</th>
                                        <th>Country</th>
                                </tr>");
            foreach (var likee in likees)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", likee.KownAs, likee.Created.ToShortDateString(), likee.DateOfBirth.CalculateAge(), likee.Country);
            }

            sb.Append(@"     </table>                   
                            </body>
                        </html>");

            return sb.ToString();
        }
    }
}