import { PageHeader } from '../components/PageHeader';
import { UserContent } from '../components/UserContent';

export function UsersPage() {
  return (
    <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <PageHeader
        title="Organizers"
        description="Meeting room users and their active reservation counts"
      />
      <UserContent />
    </main>
  );
}
