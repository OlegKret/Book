using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Dtos;
using Book.Models;
using Book.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController : ControllerBase
    {

        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;

        public ReviewersController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        //api/reviewers
        /// <summary>
        /// Get Reviewers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new List<ReviewerDto>();

            foreach (var reviewer in reviewers)
            {
                reviewerDto.Add(new ReviewerDto
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName = reviewer.LastName
                });
            }

            return Ok(reviewerDto);
        }

        //api/reviewer/reviewerId
        /// <summary>
        /// Get Reviewers
        /// </summary>
        /// <param name="reviewerId"></param>
        /// <returns></returns>
        [HttpGet("{reviewerId}", Name = "GetReviewer")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
        }

        //api/reviewers/reviewerId/reviews
        /// <summary>
        /// Get Reviews for Reviewers
        /// </summary>
        /// <param name="reviewerId"></param>
        /// <returns></returns>
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsByReviwer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto()
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }

            return Ok(reviewsDto);
        }

        //api/reviewers/reviewId/book
        /// <summary>
        /// Get Reviewer of A Review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpGet("{reviewId}/reviewer")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewerOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
        }



        //{
        //    "FirstName": "Neo",
        //    "LastName": "Andersen"
        //}

    //    {
    //        "headline": "Noe's review for Awesome Programming Book!",
    //        "reviewText": "Noes review text Reviewing Pavols Best Book and it is awesome beyond words!",
    //        "rating": 5,
    //        "Reviewer":{
    //            "Id":8,
    //            "FirstName": "A",
    //            "LastName": "B"
    //        },
    //        "Book":{
    //"Id":1,
    //            "ISBN":"123456",
    //            "Title":"ABC"
    //        }
    //    }
    //api/reviewers
    /// <summary>
    /// Create Reviewer
    /// </summary>
    /// <param name="reviewerToCreate"></param>
    /// <returns></returns>
    [HttpPost]
        [ProducesResponseType(201, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateReviewer([FromBody] Reviewer reviewerToCreate)
        {
            if (reviewerToCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.CreateReviewer(reviewerToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving " +
                                             $"{reviewerToCreate.FirstName} {reviewerToCreate.LastName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReviewer", new { reviewerId = reviewerToCreate.Id }, reviewerToCreate);
        }


        //{
        //    "Id":8,
        //    "FirstName":"Morpheus",
        //    "LastName":"Andersen"
        //}
    //api/reviewers/reviewerId
    /// <summary>
    /// Update Reviewer
    /// </summary>
    /// <param name="reviewerId"></param>
    /// <param name="updatedReviewerInfo"></param>
    /// <returns></returns>
    [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] Reviewer updatedReviewerInfo)
        {
            if (updatedReviewerInfo == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewerInfo.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.UpdateReviewer(updatedReviewerInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating " +
                                             $"{updatedReviewerInfo.FirstName} {updatedReviewerInfo.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/reviewers/reviewerId
        /// <summary>
        /// Delete Reviewer
        /// </summary>
        /// <param name="reviewerId"></param>
        /// <returns></returns>
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            var reviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting " +
                                             $"{reviewerToDelete.FirstName} {reviewerToDelete.LastName}");
                return StatusCode(500, ModelState);
            }

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", $"Something went wrong deleting reviews by" +
                                             $"{reviewerToDelete.FirstName} {reviewerToDelete.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
