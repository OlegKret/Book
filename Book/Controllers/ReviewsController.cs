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
    public class ReviewsController : ControllerBase
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;
        private IBookRepository _bookRepository;

        public ReviewsController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository, IBookRepository bookRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
        }

        //api/reviews
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }

            return Ok(reviewsDto);
        }

        //api/reviews/reviewId
        /// <summary>
        /// Get Reviews
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpGet("{reviewId}", Name = "GetReview")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewDto = new ReviewDto()
            {
                Id = review.Id,
                Headline = review.Headline,
                Rating = review.Rating,
                ReviewText = review.ReviewText
            };

            return Ok(reviewDto);
        }

        //api/reviews/books/bookId
        /// <summary>
        /// Get Reviews for a book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsForABook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

                var reviews = _reviewRepository.GetReviewsOfABook(bookId);

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

        //api/reviews/reviewId/book
        /// <summary>
        /// Get Book of A Review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpGet("{reviewId}/book")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBookOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var book = _reviewRepository.GetBookOfAReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto()
            {
                Id = book.Id,
                Title = book.Title,
                Isbm = book.Isbn,
                DatePublished = book.DatePublished
            };

            return Ok(bookDto);
        }



        //{

        //    "headline": "Test1, Test1Test1Test1Test1Test1Test1",
        //    "reviewText": "TestTest1Test1Test1Test1Test1Test1Test1Test1Test1Test1Test1Test1",
        //    "rating": 5,
        //    "Reviewer":{
        //        "Id":4,
        //        "FirstName":"A",
        //        "LastName":"B"
        //    },
        //    "Book":{
        //        "Id":1,
        //        "ISBN":"123456",
        //        "Title":"ABC"
        //    }
        //}
        //api/reviews
        /// <summary>
        /// Create Review
        /// </summary>
        /// <param name="reviewToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview([FromBody] Review reviewToCreate)
        {
            if (reviewToCreate == null)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewToCreate.Reviewer.Id))
                ModelState.AddModelError("", "Reviewer doesn't exist!");

            if (!_bookRepository.BookExists(reviewToCreate.Book.Id))
                ModelState.AddModelError("", "Book doesn't exist!");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            reviewToCreate.Book = _bookRepository.GetBook(reviewToCreate.Book.Id);
            reviewToCreate.Reviewer = _reviewerRepository.GetReviewer(reviewToCreate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReview", new { reviewId = reviewToCreate.Id }, reviewToCreate);
        }



        //{
        //    "Id":8,
        //    "headline": "Update Test1, Test1Test1Test1Test1Test1Test1",
        //    "reviewText": "Update TestTest1Test1Test1Test1Test1Test1Test1Test1Test1Test1Test1Test1",
        //    "rating": 5,
        //    "Reviewer":{
        //        "Id":4,
        //        "FirstName":"A",
        //        "LastName":"B"
        //    },
        //    "Book":{
        //        "Id":1,
        //        "ISBN":"123456",
        //        "Title":"ABC"
        //    }
        //}
    //api/reviews/reviewId
    /// <summary>
    /// Update Review
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="reviewToUpdate"></param>
    /// <returns></returns>
    [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReview(int reviewId, [FromBody] Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewToUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                ModelState.AddModelError("", "Review doesn't exist!");

            if (!_reviewerRepository.ReviewerExists(reviewToUpdate.Reviewer.Id))
                ModelState.AddModelError("", "Reviewer doesn't exist!");

            if (!_bookRepository.BookExists(reviewToUpdate.Book.Id))
                ModelState.AddModelError("", "Book doesn't exist!");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            reviewToUpdate.Book = _bookRepository.GetBook(reviewToUpdate.Book.Id);
            reviewToUpdate.Reviewer = _reviewerRepository.GetReviewer(reviewToUpdate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating the review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        //api/reviews/reviewId
        /// <summary>
        /// Delete Review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
