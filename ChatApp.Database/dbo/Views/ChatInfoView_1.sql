CREATE view dbo.ChatInfoView as
    Select C.Id                  as ConversationId,
           CI.Id                 as ChatInfoId,
           CI.Title,
           CI.ChatLink,
           CI.IsPrivate,
           (SELECT COUNT(*)
            FROM Participations P
            WHERE P.ConversationId = C.Id
              AND P.HasLeft = 0) as ParticipationsCount
    From dbo.Conversations C
             JOIN ChatInfos CI on CI.Id = C.ChatInfoId
    GROUP BY C.Id, CI.Title, CI.ChatLink, CI.IsPrivate, CI.Id