using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoApp.Models;

class VideoModel
{
    public required Uri Source { get; set; }

    public int Likes { get; set; }

    public int Loves { get; set; }

    public static VideoModel[] All { get; } =
    {
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238429360-ada73363-e4f7-41af-90d0-7c887afa6cc9.mp4"), Likes = 12, Loves = 33 },
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238428892-8ecba5b6-52af-4e47-b8df-35981cf78305.mp4"), Likes = 2, Loves = 233 },
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238428929-2a84a3f2-ae11-48bf-b35a-f8303af455a3.mp4"), Likes = 4, Loves = 4564 },
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238428943-1700a760-fdbd-4046-9166-262d84b53ae5.mp4"), Likes = 444, Loves = 6786 },
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238429048-d014b852-6ff3-4fe6-9fc5-4716e7963427.mp4"), Likes = 233, Loves = 7868 },
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238429079-6f40b68b-fc2e-452f-9188-04d943f34d72.mp4"), Likes = 12312, Loves = 64 },
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238429978-630927d6-00d4-4a26-961e-543397f04573.mp4"), Likes = 56756, Loves = 234 },
        new VideoModel { Source = new Uri("https://github-production-user-asset-6210df.s3.amazonaws.com/10573253/238430133-a4c1ac4f-828c-441f-8e18-06886d086278.mp4"), Likes = 3223, Loves = 1231 },
    };
};
