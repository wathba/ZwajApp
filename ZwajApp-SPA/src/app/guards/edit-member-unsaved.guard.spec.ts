import { TestBed, async, inject } from '@angular/core/testing';

import { EditMemberUnsavedGuard } from './edit-member-unsaved.guard';

describe('EditMemberUnsavedGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EditMemberUnsavedGuard]
    });
  });

  it('should ...', inject([EditMemberUnsavedGuard], (guard: EditMemberUnsavedGuard) => {
    expect(guard).toBeTruthy();
  }));
});
