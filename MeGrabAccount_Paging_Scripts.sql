

DELIMITER $$
CREATE PROCEDURE Paging(in tableName varchar, in whereClip varchar, in orderBy varchar, in skip int, in take int)
BEGIN

declare ids varchar(1000);
declare _id varchar(40);
declare done int;

DECLARE id_cursor CURSOR FOR SELECT rpga_id FROM redpacket_grab_activity order by rpga_start_datetime desc LIMIT 100, 2;

DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;

open id_cursor;

cursor_loop:loop
	FETCH id_cursor into _id; 
    
    set ids = _id;
    
	if done = 1 then
		leave cursor_loop;
	end if;

end loop cursor_loop;

close id_cursor;

select ids;

select * from redpacket_grab_activity where rpga_id in (ids);

END

DELIMITER ;

call Paging();

DROP PROCEDURE IF EXISTS Paging


