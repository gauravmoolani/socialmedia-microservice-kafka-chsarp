using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using Post.Common.Events;
namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {


        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });

        }
        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;

        }
        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive  post");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty");
            }
            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message

            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;


        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like a inactive post");
            }
            RaiseEvent(new PostLikedEvent
            {
                Id = _id

            });

        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }
        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot add a comment to an inactive post !");
            }
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty");
            }
            RaiseEvent(new CommentAddedEvent
            {
                Comment = comment,
                Id = _id,
                CommentId = Guid.NewGuid(),
                Username = username,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));


        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot add a comment to an inactive post !");
            }
            if (_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty");
            }
            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.Now

            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot add a comment to an inactive post !");
            }
            if (_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"you cannot remove comment to an another  post");
            }

            RaiseEvent(new CommentRemovedEvent
            {
                CommentId = commentId,
                Id = _id,

            });

        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }
        public void DeletPost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot add a comment to an inactive post !");
            }
            if (_author != username)
            {
                throw new InvalidOperationException($"you cannot remove post of another user");
            }
            RaiseEvent(new PostRemovedEvent
            {
                Id = _id

            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;

        }







    }
}