CREATE view dbo.ChatInfoView as
    Select C.Id                  as ConversationId,
           CI.Id                 as ChatInfoId,
           CI.Title,
           CI.ChatLink,
           CI.IsPrivate,
           COUNT(P.AspNetUserId) as ParticipationsCount
    From dbo.Conversations C
             JOIN Participations P on C.Id = P.ConversationId
             JOIN ChatInfos CI on CI.Id = C.ChatInfoId
    WHERE P.HasLeft = 0
    GROUP BY C.Id, CI.Title, CI.ChatLink, CI.IsPrivate, CI.Id