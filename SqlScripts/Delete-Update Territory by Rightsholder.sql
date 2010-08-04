delete from dbo.ContentRightTerritories 
from 
dbo.ContentRightTerritories crt 
inner join dbo.ContentRights cr on crt.ContentRightId = cr.ContentRightId
where
cr.RightsHolderName = 'Ford Music Services'
and crt.TerritoryId = 1
